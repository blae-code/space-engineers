import * as THREE from 'three';

// Loaded only with ?test=1. Puts the studio into a requested state, then publishes
// render stats and palette-slot contrast for tests/visual/run_visual.py.
//   ?test=1&model=mastodon&step=last&mode=x-ray&cam=iso
// step: first | mid | last | <0-based index> | <step id such as 10.4>

const MASK_OTHER = [0, 255, 0];
const MASK_TOLERANCE = 3;

function maskColorForSlot(slot) {
  return [30 + slot * 22, 0, 255];
}

function nextFrame() {
  return new Promise((resolve) => requestAnimationFrame(() => resolve()));
}

function srgbToLinear(channel) {
  const c = channel / 255;
  return c <= 0.04045 ? c / 12.92 : ((c + 0.055) / 1.055) ** 2.4;
}

// WCAG relative luminance of an sRGB pixel.
function relativeLuminance(r, g, b) {
  return 0.2126 * srgbToLinear(r) + 0.7152 * srgbToLinear(g) + 0.0722 * srgbToLinear(b);
}

function contrastRatio(a, b) {
  const hi = Math.max(a, b);
  const lo = Math.min(a, b);
  return (hi + 0.05) / (lo + 0.05);
}

function resolveStepIndex(steps, value) {
  if (value === null || value === '' || value === 'first') return 0;
  if (value === 'last') return steps.length - 1;
  if (value === 'mid') return Math.floor((steps.length - 1) / 2);
  const byId = steps.findIndex((step) => String(step.step) === value);
  if (byId !== -1) return byId;
  const index = Number.parseInt(value, 10);
  if (Number.isInteger(index) && index >= 0 && index < steps.length) return index;
  throw new Error(`Unknown step "${value}"`);
}

function readCanvasPixels(canvas) {
  const copy = document.createElement('canvas');
  copy.width = canvas.width;
  copy.height = canvas.height;
  const ctx = copy.getContext('2d', { willReadFrequently: true });
  ctx.drawImage(canvas, 0, 0);
  return ctx.getImageData(0, 0, copy.width, copy.height);
}

function matches(data, offset, color) {
  return Math.abs(data[offset] - color[0]) <= MASK_TOLERANCE &&
    Math.abs(data[offset + 1] - color[1]) <= MASK_TOLERANCE &&
    Math.abs(data[offset + 2] - color[2]) <= MASK_TOLERANCE;
}

// Renders a flat, unlit mask (each solid palette slot in its own color), reads it,
// then renders the normal frame and measures luminance under each eroded mask region.
function measureSlotContrast(world) {
  const { renderer, scene, camera, rootGroup } = world;
  const saved = {
    background: scene.background,
    fog: scene.fog,
    toneMapping: renderer.toneMapping,
    visibility: [],
    materials: []
  };
  const maskMaterials = new Map();
  const maskMaterial = (rgb) => {
    const key = rgb.join(',');
    if (!maskMaterials.has(key)) {
      const color = new THREE.Color().setRGB(rgb[0] / 255, rgb[1] / 255, rgb[2] / 255, THREE.SRGBColorSpace);
      maskMaterials.set(key, new THREE.MeshBasicMaterial({ color, toneMapped: false, clippingPlanes: world.clipPlanes }));
    }
    return maskMaterials.get(key);
  };

  const slotsPresent = new Set();
  scene.traverse((node) => {
    if (node === scene) return;
    const isBlock = node.parent === rootGroup;
    const isBlockChild = node.parent && node.parent.parent === rootGroup;
    if (isBlock) {
      if (!node.visible) return;
      saved.materials.push([node, node.material]);
      const slot = node.userData.paletteSlot;
      const isSolid = node.material === world.paletteMaterials[slot];
      if (isSolid) slotsPresent.add(slot);
      node.material = maskMaterial(isSolid ? maskColorForSlot(slot) : MASK_OTHER);
    } else if (isBlockChild || (node !== rootGroup && !node.isLight)) {
      saved.visibility.push([node, node.visible]);
      node.visible = false;
    }
  });
  scene.background = new THREE.Color(0x000000);
  scene.fog = null;
  renderer.toneMapping = THREE.NoToneMapping;

  renderer.render(scene, camera);
  const mask = readCanvasPixels(renderer.domElement);

  saved.materials.forEach(([node, material]) => { node.material = material; });
  saved.visibility.forEach(([node, visible]) => { node.visible = visible; });
  scene.background = saved.background;
  scene.fog = saved.fog;
  renderer.toneMapping = saved.toneMapping;
  maskMaterials.forEach((material) => material.dispose());

  renderer.render(scene, camera);
  const frame = readCanvasPixels(renderer.domElement);

  const { width, height } = mask;
  const regions = [{ key: 'background', color: [0, 0, 0] }];
  slotsPresent.forEach((slot) => regions.push({ key: `slot${slot}`, slot, color: maskColorForSlot(slot) }));

  const result = { width, height, slots: {} };
  let backgroundLuminance = null;
  regions.forEach((region) => {
    const values = [];
    for (let y = 1; y < height - 1; y++) {
      for (let x = 1; x < width - 1; x++) {
        // Erode by one pixel so anti-aliased edges never count toward a region.
        let inside = true;
        for (let dy = -1; dy <= 1 && inside; dy++) {
          for (let dx = -1; dx <= 1 && inside; dx++) {
            inside = matches(mask.data, ((y + dy) * width + (x + dx)) * 4, region.color);
          }
        }
        if (!inside) continue;
        const o = (y * width + x) * 4;
        values.push(relativeLuminance(frame.data[o], frame.data[o + 1], frame.data[o + 2]));
      }
    }
    if (values.length === 0) return;
    values.sort((a, b) => a - b);
    const mean = values.reduce((sum, v) => sum + v, 0) / values.length;
    const p90 = values[Math.floor(values.length * 0.9)];
    if (region.key === 'background') {
      backgroundLuminance = mean;
      result.background = { pixels: values.length, meanLuminance: mean };
    } else {
      result.slots[region.key] = { slot: region.slot, pixels: values.length, meanLuminance: mean, p90Luminance: p90 };
    }
  });

  if (backgroundLuminance !== null) {
    Object.values(result.slots).forEach((entry) => {
      entry.contrastMean = contrastRatio(entry.meanLuminance, backgroundLuminance);
      entry.contrastP90 = contrastRatio(entry.p90Luminance, backgroundLuminance);
    });
  }
  return result;
}

function renderStats(world) {
  world.controls.update();
  world.renderer.render(world.scene, world.camera);
  const { render, memory } = world.renderer.info;
  return {
    drawCalls: render.calls,
    triangles: render.triangles,
    lines: render.lines,
    points: render.points,
    geometries: memory.geometries,
    textures: memory.textures,
    canvas: [world.renderer.domElement.width, world.renderer.domElement.height]
  };
}

function clickButton(selector) {
  const button = document.querySelector(selector);
  if (!button) throw new Error(`No control matches ${selector}`);
  button.click();
}

export async function installTestHarness(app, params) {
  const out = document.createElement('pre');
  out.id = '__stats';
  out.hidden = true;
  document.body.appendChild(out);

  const publish = (payload, ready) => {
    out.textContent = JSON.stringify(payload);
    document.body.dataset.ready = ready;
  };

  try {
    const modelParam = params.get('model');
    if (modelParam) {
      const select = document.getElementById('chapter-select');
      const wanted = modelParam.toLowerCase();
      const option = [...select.options].find((o) =>
        o.value.toLowerCase() === wanted || o.value.toLowerCase() === `${wanted}.json`);
      if (!option) throw new Error(`Unknown model "${modelParam}"`);
      if (select.value !== option.value || !app.modelData) {
        select.value = option.value;
        await app.loadBlueprint(option.value);
      }
    }
    if (!app.modelData) throw new Error('No model loaded');

    app.updateStep(resolveStepIndex(app.modelData.steps, params.get('step')));
    if (params.get('mode')) clickButton(`.mode-btn[data-mode="${params.get('mode')}"]`);
    if (params.get('cam')) clickButton(`.camera-btn[data-cam="${params.get('cam')}"]`);

    await nextFrame();
    await nextFrame();

    const world = app.voxelWorld;
    const step = app.modelData.steps[app.currentStepIdx];
    const payload = {
      model: app.modelData.id,
      stepIndex: app.currentStepIdx,
      stepId: step.step,
      mode: world.renderMode,
      cam: world.cameraMode,
      stats: renderStats(world)
    };
    if (params.get('contrast') !== '0') payload.contrast = measureSlotContrast(world);

    window.__studio = {
      stats: () => renderStats(world),
      contrast: () => measureSlotContrast(world),
      state: () => ({ model: app.modelData.id, stepIndex: app.currentStepIdx, mode: world.renderMode, cam: world.cameraMode })
    };
    publish(payload, '1');
  } catch (error) {
    console.error('Test harness failed:', error);
    publish({ error: String(error && error.message || error) }, 'error');
  }
}
