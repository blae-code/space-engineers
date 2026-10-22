import { VoxelWorld } from './VoxelWorld.js';
import { Slicer2D } from './Slicer2D.js';
import { SnapshotApi } from './SnapshotApi.js';

class AppController {
  constructor() {
    this.currentStepIdx = 0;
    this.modelData = null;
    this.activeLevel = 'L2';
    this.storageWarningShown = false;
    this.blueprintRequestId = 0;
    this.installPromptEvent = null;
    this.serviceWorkerRegistration = null;
    this.applyingServiceWorkerUpdate = false;
    this.didReloadForServiceWorkerUpdate = false;
    this.init();
  }

  async init() {
    const params = new URLSearchParams(window.location.search);
    const testMode = params.get('test') === '1';

    this.canvasContainer = document.getElementById('canvas-container');
    this.voxelWorld = new VoxelWorld(this.canvasContainer, {
      onBlockSelected: (data) => this.handleBlockSelected(data)
    }, { testMode });

    this.bindEvents();
    // Test runs skip the service worker so no cached state leaks between captures.
    if (!testMode) {
      this.bindPwaEvents();
      this.registerServiceWorker();
    } else {
      document.querySelector('.pwa-controls')?.setAttribute('hidden', '');
    }

    await this.loadChapterRegistry();

    if (testMode) {
      const { installTestHarness } = await import('./TestHarness.js');
      await installTestHarness(this, params);
    }
  }

  async loadChapterRegistry() {
    const status = document.getElementById('load-status');
    try {
      const response = await fetch('./data/chapters.json');
      if (!response.ok) throw new Error(`Chapter registry request failed (${response.status})`);
      const registry = await response.json();
      if (!Array.isArray(registry) || registry.length === 0 || registry.some((chapter) =>
        !chapter || typeof chapter.file !== 'string' || !/^[a-zA-Z0-9_-]+\.json$/.test(chapter.file) ||
        typeof chapter.name !== 'string' || !Number.isInteger(chapter.chapter)
      )) {
        throw new Error('Chapter registry is empty or malformed');
      }
      const select = document.getElementById('chapter-select');
      select.replaceChildren(...registry.map((chapter) => {
        const option = document.createElement('option');
        option.value = chapter.file;
        option.textContent = `CH ${chapter.chapterLabel || chapter.chapter} · ${chapter.name}`;
        return option;
      }));
      const initial = registry.find((chapter) => chapter.id === 'MAS-01') || registry[0];
      select.value = initial.file;
      await this.loadBlueprint(initial.file);
    } catch (error) {
      console.error('Failed to load chapter registry:', error);
      if (status) status.textContent = `Unable to load chapter registry: ${error.message}`;
      document.getElementById('hud-step-desc').textContent = 'Chapter registry unavailable';
    }
  }

  handleBlockSelected(data) {
    const card = document.getElementById('selected-block-card');
    if (!card) return;

    if (!data) {
      card.style.display = 'none';
      return;
    }

    card.style.display = 'flex';
    document.getElementById('sel-block-name').textContent = data.name || 'Block';
    document.getElementById('sel-block-subtype').textContent = data.subtype || data.shape;
    document.getElementById('sel-block-coords').textContent = `[${data.pos.join(', ')}]`;
    document.getElementById('sel-block-subsystem').textContent = (data.subsystem || 'structure').toUpperCase();
    document.getElementById('sel-block-palette').textContent = `Slot ${data.paletteSlot} (${data.paletteName || ''})`;
    document.getElementById('sel-block-step').textContent = `Step ${data.stepId || data.stepIndex + 1}`;
    document.getElementById('sel-block-phase').textContent = `Phase ${data.phase || ''}`;
  }

  updateClippingBounds() {
    if (!this.modelData || !this.modelData.bounds) return;
    const b = this.modelData.bounds;

    const clipX = document.getElementById('clip-x');
    const clipY = document.getElementById('clip-y');
    const clipZ = document.getElementById('clip-z');

    if (clipX) {
      clipX.min = b.min[0];
      clipX.max = b.max[0] + 1;
      clipX.value = clipX.max;
    }
    if (clipY) {
      clipY.min = b.min[1];
      clipY.max = b.max[1] + 1;
      clipY.value = clipY.max;
    }
    if (clipZ) {
      clipZ.min = b.min[2];
      clipZ.max = b.max[2] + 1;
      clipZ.value = clipZ.max;
    }

    const clipXVal = document.getElementById('clip-x-val');
    const clipYVal = document.getElementById('clip-y-val');
    const clipZVal = document.getElementById('clip-z-val');
    if (clipXVal) clipXVal.textContent = 'OFF';
    if (clipYVal) clipYVal.textContent = 'OFF';
    if (clipZVal) clipZVal.textContent = 'OFF';

    this.voxelWorld.setClipping('all', null, false);
  }

  async loadBlueprint(url) {
    const requestId = ++this.blueprintRequestId;
    const status = document.getElementById('load-status');
    if (status) status.textContent = `Loading ${url}…`;
    try {
      const response = await fetch(`./data/${encodeURIComponent(url)}`);
      if (!response.ok) throw new Error(`Blueprint request failed (${response.status})`);
      const model = await response.json();
      if (requestId !== this.blueprintRequestId) return;
      if (!model || !Array.isArray(model.steps) || !Array.isArray(model.palette) || !model.bounds) {
        throw new Error('Blueprint data is incomplete or malformed');
      }
      this.modelData = model;
      this.totalModelPlacements = model.steps.reduce((total, step) =>
        total + (Array.isArray(step.blocks) ? step.blocks.length : 0), 0);
      this.voxelWorld.loadModel(this.modelData);

      this.initStepSlider();
      this.initSubsystemToggles();
      this.updateClippingBounds();
      this.updateStep(0);
      this.updateSliceView();
      this.updateCollabStats();
      if (status) status.textContent = '';
    } catch (error) {
      if (requestId !== this.blueprintRequestId) return;
      console.error(`Failed to load blueprint ${url}:`, error);
      if (status) status.textContent = `Unable to load ${url}: ${error.message}`;
      document.getElementById('hud-step-desc').textContent = 'Blueprint unavailable';
    }
  }

  initStepSlider() {
    const slider = document.getElementById('step-slider');
    slider.min = 0;
    slider.max = Math.max(0, this.modelData.steps.length - 1);
    slider.value = 0;

    slider.oninput = (e) => {
      this.updateStep(parseInt(e.target.value, 10));
    };
  }

  initSubsystemToggles() {
    const toggles = document.querySelectorAll('.chip-toggle[data-sub]');
    toggles.forEach((btn) => {
      btn.onclick = () => {
        const sub = btn.dataset.sub;
        const isActive = btn.classList.toggle('active');
        btn.setAttribute('aria-pressed', String(isActive));
        this.voxelWorld.toggleSubsystem(sub, isActive);
      };
    });
  }

  getStorageKey(prefix, modelId = this.modelData?.id) {
    return `se3d_${prefix}_${modelId || 'default'}`;
  }

  getStoredChecks() {
    try {
      const val = localStorage.getItem(this.getStorageKey('checks'));
      const checks = val ? JSON.parse(val) : {};
      if (!checks || typeof checks !== 'object' || Array.isArray(checks)) {
        throw new Error('Saved verification state is not an object');
      }
      return checks;
    } catch (error) {
      this.reportStorageFailure(error);
      return {};
    }
  }

  setStoredCheck(checkKey, isChecked) {
    try {
      const checks = this.getStoredChecks();
      checks[checkKey] = isChecked;
      localStorage.setItem(this.getStorageKey('checks'), JSON.stringify(checks));
      this.updateCollabStats();
      return true;
    } catch (error) {
      this.reportStorageFailure(error);
      return false;
    }
  }

  getStoredNote(stepId, modelId = this.modelData?.id) {
    try {
      return localStorage.getItem(`${this.getStorageKey('note', modelId)}_${stepId}`) || '';
    } catch (error) {
      this.reportStorageFailure(error);
      return '';
    }
  }

  setStoredNote(stepId, noteText, modelId = this.modelData?.id) {
    try {
      localStorage.setItem(`${this.getStorageKey('note', modelId)}_${stepId}`, noteText);
      this.updateCollabStats();
      return true;
    } catch (error) {
      this.reportStorageFailure(error);
      return false;
    }
  }

  reportStorageFailure(error) {
    console.warn('Local collaboration state could not be read or saved:', error);
    if (this.storageWarningShown) return;
    this.storageWarningShown = true;
    const status = document.getElementById('notes-status');
    if (status) status.textContent = 'Local storage unavailable; changes may not be saved.';
  }

  updateCollabStats() {
    const checks = this.getStoredChecks();
    let verifiedCount = Object.values(checks).filter(Boolean).length;
    let totalChecks = 0;
    this.modelData?.steps?.forEach((s) => {
      totalChecks += (s.checks || []).length;
    });

    const verifiedEl = document.getElementById('collab-verified-count');
    if (verifiedEl) {
      verifiedEl.textContent = `${verifiedCount} / ${totalChecks}`;
    }

    let notesCount = 0;
    this.modelData?.steps?.forEach((s) => {
      if (this.getStoredNote(s.step).trim().length > 0) notesCount++;
    });
    const notesEl = document.getElementById('collab-notes-count');
    if (notesEl) {
      notesEl.textContent = `${notesCount}`;
    }
  }

  updateStep(idx) {
    idx = Math.max(0, Math.min(idx, this.modelData.steps.length - 1));
    this.currentStepIdx = idx;
    document.getElementById('step-slider').value = idx;
    this.voxelWorld.setStep(idx);

    const step = this.modelData.steps[idx];
    if (!step) return;

    // Update HUD
    document.getElementById('hud-step-badge').textContent = `STEP ${step.step}`;
    document.getElementById('hud-step-desc').textContent = `${step.title} (${step.level})`;
    document.getElementById('hud-phase-badge').textContent = `PHASE ${step.phase}`;
    document.getElementById('step-slider').setAttribute('aria-valuetext', `Step ${step.step}: ${step.title}`);
    document.getElementById('btn-prev-step').disabled = idx === 0;
    document.getElementById('btn-next-step').disabled = idx === this.modelData.steps.length - 1;

    // Update Inspector Sidebar Header
    document.getElementById('meta-model-name').textContent = this.modelData.name;
    document.getElementById('meta-step-title').textContent = step.title;
    document.getElementById('meta-phase').textContent = `PHASE ${step.phase}`;
    document.getElementById('meta-level').textContent = `LEVEL ${step.level}`;
    document.getElementById('meta-grid-metric').textContent = `${this.modelData.gridSize} (${this.modelData.blockMeters} m)`;
    this.activeLevel = step.level.split('-')[0] || 'L2';

    // Calculate current cumulative stats
    let totalBlocks = 0;
    for (let i = 0; i <= idx; i++) {
      totalBlocks += this.modelData.steps[i].blocks.length;
    }
    const stats = this.modelData.stats || {};
    const buildBlocks = Number(stats.totalBlocks);
    document.getElementById('meta-blocks-count').textContent =
      `${totalBlocks} / ${this.totalModelPlacements} preview placements · ${
        Number.isFinite(buildBlocks) ? `~${buildBlocks.toLocaleString()} full-build blocks (unverified)` : 'full-build count unavailable'
      }`;
    document.getElementById('meta-mass').textContent =
      Number.isFinite(Number(stats.dryMassTonnes))
        ? `~${Number(stats.dryMassTonnes).toLocaleString()} t · unverified target`
        : 'Unverified target unavailable';
    document.getElementById('meta-pcu').textContent =
      Number.isFinite(Number(stats.pcu))
        ? `~${Number(stats.pcu).toLocaleString()} · unverified target`
        : 'Unverified target unavailable';
    document.getElementById('meta-payload').textContent =
      Number.isFinite(Number(stats.orePayloadTonnes))
        ? `${Number(stats.orePayloadTonnes).toLocaleString()} t · unverified target`
        : 'Unverified target unavailable';
    document.getElementById('meta-tool-reach').textContent =
      Number.isFinite(Number(stats.toolReachTargetMeters))
        ? `${Number(stats.toolReachTargetMeters).toLocaleString()} m · unverified target`
        : 'Unverified target unavailable';
    const powerTargets = [
      Number.isFinite(Number(stats.powerBaseloadMW)) ? `${stats.powerBaseloadMW} MW generation` : null,
      Number.isFinite(Number(stats.powerPeakMW)) ? `${stats.powerPeakMW} MW peak discharge` : null,
      Number.isFinite(Number(stats.batteryCapacityMWh)) ? `${stats.batteryCapacityMWh} MWh storage` : null
    ].filter(Boolean);
    document.getElementById('meta-power-targets').textContent =
      powerTargets.length > 0 ? `${powerTargets.join(' / ')} · unverified` : 'Unverified targets unavailable';
    const dimensions = stats.dimensionsBlocks;
    document.getElementById('meta-envelope').textContent =
      Array.isArray(dimensions) && dimensions.length === 3
        ? `${dimensions.join(' × ')} B · target`
        : 'Unverified target unavailable';
    const designWarning = this.modelData.designFigureStatus?.label ||
      'Model placements and design figures are not a validated in-game blueprint.';
    document.getElementById('meta-design-warning').textContent = designWarning;
    this.updateDesignInterfaces(step);

    // Update Parts Tray
    const partsBody = document.getElementById('parts-tray-body');
    partsBody.innerHTML = '';
    if (step.partsTray && step.partsTray.length > 0) {
      step.partsTray.forEach((p) => {
        const pal = this.modelData.palette?.find((item) => item.slot === Number(p.slot));
        const row = document.createElement('tr');
        const typeCell = row.insertCell();
        const type = document.createElement('strong');
        type.textContent = p.type || 'Unspecified block';
        typeCell.appendChild(type);
        const quantityCell = row.insertCell();
        quantityCell.className = 'mono';
        quantityCell.textContent = String(p.qty ?? '');
        const paletteCell = row.insertCell();
        paletteCell.className = 'mono';
        paletteCell.style.whiteSpace = 'nowrap';
        if (pal) {
          const swatch = document.createElement('span');
          swatch.className = 'swatch-dot';
          swatch.style.cssText = `background:${pal.hex}; border:1px solid rgba(255,255,255,0.4); margin-right:5px; vertical-align:middle;`;
          paletteCell.appendChild(swatch);
        }
        paletteCell.append(`Slot ${p.slot ?? '—'}`);
        if (pal) {
          const paletteName = document.createElement('span');
          paletteName.style.cssText = 'color:var(--muted); font-size:10px; display:block;';
          paletteName.textContent = pal.name;
          paletteCell.appendChild(paletteName);
        }
        const roleCell = row.insertCell();
        roleCell.style.fontSize = '11px';
        roleCell.textContent = p.note || p.massPcu || '';
        partsBody.appendChild(row);
      });
    } else {
      partsBody.innerHTML = '<tr><td colspan="4" style="color:var(--muted); text-align:center;">Section framing / standard armor</td></tr>';
    }

    // Update Inspection Checklist with localStorage persistence
    const checkList = document.getElementById('inspection-checklist');
    checkList.innerHTML = '';
    const storedChecks = this.getStoredChecks();

    if (step.checks && step.checks.length > 0) {
      step.checks.forEach((c, cIdx) => {
        const checkKey = `${step.step}_${cIdx}`;
        const isChecked = !!storedChecks[checkKey];

        const item = document.createElement('label');
        item.className = 'check-item';
        const checkbox = document.createElement('input');
        checkbox.type = 'checkbox';
        checkbox.checked = isChecked;
        const text = document.createElement('span');
        text.textContent = c;
        item.append(checkbox, text);

        checkbox.addEventListener('change', (e) => {
          this.setStoredCheck(checkKey, e.target.checked);
        });

        checkList.appendChild(item);
      });
    } else {
      checkList.innerHTML = '<div style="font-size:11px; color:var(--muted);">Standard structural integrity checks apply.</div>';
    }

    // Update Placement Instructions
    const instructionsList = document.getElementById('instructions-list');
    instructionsList.innerHTML = '';
    if (step.instructions && step.instructions.length > 0) {
      step.instructions.forEach((inst) => {
        const li = document.createElement('li');
        li.textContent = inst;
        instructionsList.appendChild(li);
      });
    } else {
      instructionsList.innerHTML = '<li>Place blocks per 3D projection and transverse station profiles.</li>';
    }

    // Update Terminal Configurations
    const termConfigEl = document.getElementById('terminal-config-text');
    if (step.terminalConfig) {
      termConfigEl.textContent = step.terminalConfig;
      termConfigEl.parentElement.style.display = 'block';
    } else {
      termConfigEl.parentElement.style.display = 'none';
    }

    // Update Collaborative Notes
    const notesInput = document.getElementById('step-notes-input');
    notesInput.value = this.getStoredNote(step.step);

    this.updateSliceView();
  }

  updateDesignInterfaces(step) {
    const card = document.getElementById('design-interfaces-card');
    const list = document.getElementById('design-interfaces-list');
    if (!card || !list) return;

    const stepId = String(step.step);
    const systems = (this.modelData.designSubsystems || [])
      .filter((system) => Array.isArray(system.steps) && system.steps.includes(stepId));
    list.replaceChildren();

    if (systems.length === 0) {
      card.style.display = 'none';
      return;
    }

    card.style.display = 'block';
    systems.forEach((system) => {
      const entry = document.createElement('section');
      entry.className = 'design-interface-entry';
      const heading = document.createElement('h4');
      heading.textContent = system.label;
      entry.appendChild(heading);

      [
        ['Physical interface', system.physicalInterface],
        ['Future PB responsibility', system.futurePB],
        ['Manual fallback', system.manualFallback],
        ['Commissioning gate', system.commissioning]
      ].forEach(([label, value]) => {
        if (typeof value !== 'string' || !value.trim()) return;
        const row = document.createElement('p');
        const key = document.createElement('strong');
        key.textContent = `${label}: `;
        row.append(key, document.createTextNode(value));
        entry.appendChild(row);
      });
      list.appendChild(entry);
    });
  }

  updateSliceView() {
    const level = this.activeLevel;
    const svgWrap = document.getElementById('slice-svg-container');
    const asciiWrap = document.getElementById('slice-ascii-container');

    svgWrap.innerHTML = Slicer2D.generateSvgSlice(this.modelData, level, this.currentStepIdx);
    asciiWrap.textContent = Slicer2D.generateAsciiSlice(this.modelData, level, this.currentStepIdx);
    document.getElementById('slice-level-title').textContent = `LEVEL ${level} FLOORPLAN`;
  }

  exportCollaborationLog() {
    const storedChecks = this.getStoredChecks();
    let lines = [];
    lines.push(`# Collaboration & Verification Log · ${this.modelData.name} (${this.modelData.id})`);
    lines.push(`Generated: ${new Date().toISOString()} · Space Engineers 3D Studio\n`);
    lines.push(`## Model Overview`);
    lines.push(`* **Grid Metric:** ${this.modelData.gridSize} (${this.modelData.blockMeters} m)`);
    lines.push(`* **Spatial Preview Placements:** ${this.totalModelPlacements}`);
    lines.push(`* **Design Figure Status:** ${this.modelData.designFigureStatus?.label || 'Unverified'}\n`);
    lines.push(`* **Estimated Full-Build Blocks:** ~${this.modelData.stats.totalBlocks} (unverified)`);
    lines.push(`* **Dry Mass Target:** ~${this.modelData.stats.dryMassTonnes} t (unverified)`);
    lines.push(`* **PCU Target:** ~${this.modelData.stats.pcu} (unverified)`);
    lines.push(`* **Payload Target:** ${this.modelData.stats.orePayloadTonnes} t (unverified)`);
    lines.push(`* **Tool-Reach Target:** ${this.modelData.stats.toolReachTargetMeters} m (unverified)`);
    lines.push(`* **Power/Storage Targets:** ${this.modelData.stats.powerBaseloadMW} MW generation, ${this.modelData.stats.powerPeakMW} MW peak discharge, ${this.modelData.stats.batteryCapacityMWh} MWh (unverified)\n`);
    lines.push(`---`);

    this.modelData.steps.forEach((step) => {
      lines.push(`\n### Step ${step.step} · ${step.title} (${step.level})`);
      lines.push(`* **Phase:** ${step.phase} · **Subsystem:** ${step.subsystem}`);
      const designSubsystems = (this.modelData.designSubsystems || [])
        .filter((system) => Array.isArray(system.steps) && system.steps.includes(String(step.step)));
      designSubsystems.forEach((system) => {
        lines.push(`\n**${system.label} · Future interface (not implemented)**`);
        lines.push(`* Physical interface: ${system.physicalInterface}`);
        lines.push(`* Future PB responsibility: ${system.futurePB}`);
        lines.push(`* Manual fallback: ${system.manualFallback}`);
        lines.push(`* Commissioning gate: ${system.commissioning}`);
      });

      if (step.checks && step.checks.length > 0) {
        lines.push(`\n**Verification Checks:**`);
        step.checks.forEach((c, cIdx) => {
          const isDone = !!storedChecks[`${step.step}_${cIdx}`];
          lines.push(`* [${isDone ? 'x' : ' '}] ${c}`);
        });
      }

      const note = this.getStoredNote(step.step).trim();
      if (note) {
        lines.push(`\n> **Engineer/AI Note:**\n> ${note.replace(/\n/g, '\n> ')}`);
      }
    });

    const reportText = lines.join('\n');
    SnapshotApi.downloadFile(`${this.modelData.id}-collaboration-report.md`, reportText, 'text/markdown');
  }

  bindEvents() {
    // Chapter dropdown selector
    const chapterSelect = document.getElementById('chapter-select');
    chapterSelect.addEventListener('change', (e) => {
      clearTimeout(saveTimeout);
      this.loadBlueprint(e.target.value);
    });

    // Inspector Tab switching
    document.querySelectorAll('.rail-tab').forEach((tab) => {
      tab.addEventListener('click', () => {
        document.querySelectorAll('.rail-tab').forEach((t) => t.classList.remove('active'));
        tab.classList.add('active');
        document.querySelectorAll('.rail-tab').forEach((t) => t.setAttribute('aria-pressed', String(t === tab)));

        document.querySelectorAll('.tab-panel').forEach((p) => (p.style.display = 'none'));
        const activePanel = document.getElementById(`tab-panel-${tab.dataset.tab}`);
        if (activePanel) activePanel.style.display = 'block';
      });
    });

    // Notes auto-save input
    const notesInput = document.getElementById('step-notes-input');
    const notesStatus = document.getElementById('notes-status');
    let saveTimeout = null;

    notesInput.addEventListener('input', (e) => {
      notesStatus.textContent = 'Saving...';
      clearTimeout(saveTimeout);
      const modelId = this.modelData?.id;
      const stepId = this.modelData?.steps[this.currentStepIdx]?.step;
      const noteText = e.target.value;
      saveTimeout = setTimeout(() => {
        if (modelId && stepId !== undefined) {
          const saved = this.setStoredNote(stepId, noteText, modelId);
          if (saved && this.modelData?.id === modelId &&
              this.modelData.steps[this.currentStepIdx]?.step === stepId) {
            notesStatus.textContent = 'Saved to localStorage';
          }
        }
      }, 350);
    });

    document.getElementById('btn-clear-note').addEventListener('click', () => {
      const step = this.modelData.steps[this.currentStepIdx];
      if (step) {
        notesInput.value = '';
        if (this.setStoredNote(step.step, '')) notesStatus.textContent = 'Note cleared';
      }
    });

    // Step navigation buttons
    document.getElementById('btn-prev-step').addEventListener('click', () => {
      if (this.currentStepIdx > 0) this.updateStep(this.currentStepIdx - 1);
    });
    document.getElementById('btn-next-step').addEventListener('click', () => {
      if (this.modelData && this.currentStepIdx < this.modelData.steps.length - 1) {
        this.updateStep(this.currentStepIdx + 1);
      }
    });

    // Camera view buttons
    document.querySelectorAll('.camera-btn').forEach((btn) => {
      btn.addEventListener('click', () => {
        document.querySelectorAll('.camera-btn').forEach((b) => b.classList.remove('active'));
        btn.classList.add('active');
        document.querySelectorAll('.camera-btn').forEach((b) => b.setAttribute('aria-pressed', String(b === btn)));
        this.voxelWorld.setCameraView(btn.dataset.cam);
      });
    });

    // Render Mode buttons (Solid, Ghost, X-Ray, Wire)
    document.querySelectorAll('.mode-btn').forEach((btn) => {
      btn.addEventListener('click', () => {
        document.querySelectorAll('.mode-btn').forEach((b) => b.classList.remove('active'));
        btn.classList.add('active');
        document.querySelectorAll('.mode-btn').forEach((b) => b.setAttribute('aria-pressed', String(b === btn)));
        this.voxelWorld.setRenderMode(btn.dataset.mode);
      });
    });

    // Turntable auto-orbit toggle
    const btnOrbit = document.getElementById('btn-toggle-orbit');
    if (btnOrbit) {
      btnOrbit.addEventListener('click', () => {
        const isRotating = !this.voxelWorld.controls.autoRotate;
        this.voxelWorld.controls.autoRotate = isRotating;
        btnOrbit.classList.toggle('active', isRotating);
        btnOrbit.setAttribute('aria-pressed', String(isRotating));
      });
    }

    // Section Cutaway / Clipping Plane Controls
    const btnClip = document.getElementById('btn-toggle-clip');
    const clipPanel = document.getElementById('clipping-panel');
    const clipX = document.getElementById('clip-x');
    const clipY = document.getElementById('clip-y');
    const clipZ = document.getElementById('clip-z');
    const clipXVal = document.getElementById('clip-x-val');
    const clipYVal = document.getElementById('clip-y-val');
    const clipZVal = document.getElementById('clip-z-val');
    const btnResetClip = document.getElementById('btn-reset-clip');

    let clippingActive = false;

    if (btnClip && clipPanel) {
      btnClip.addEventListener('click', () => {
        clippingActive = !clippingActive;
        clipPanel.style.display = clippingActive ? 'flex' : 'none';
        btnClip.classList.toggle('active', clippingActive);
        btnClip.setAttribute('aria-pressed', String(clippingActive));
        if (!clippingActive) {
          this.voxelWorld.setClipping('all', null, false);
        } else {
          const xVal = parseFloat(clipX.value) >= parseFloat(clipX.max) ? null : parseFloat(clipX.value);
          const yVal = parseFloat(clipY.value) >= parseFloat(clipY.max) ? null : parseFloat(clipY.value);
          const zVal = parseFloat(clipZ.value) >= parseFloat(clipZ.max) ? null : parseFloat(clipZ.value);
          this.voxelWorld.setClipping('x', xVal, true);
          this.voxelWorld.setClipping('y', yVal, true);
          this.voxelWorld.setClipping('z', zVal, true);
        }
      });
    }

    if (clipX) {
      clipX.addEventListener('input', (e) => {
        const val = parseFloat(e.target.value);
        if (val >= parseFloat(clipX.max)) {
          clipXVal.textContent = 'OFF';
          this.voxelWorld.setClipping('x', null, true);
        } else {
          clipXVal.textContent = `${val >= 0 ? '+' : ''}${val.toFixed(1)}B`;
          this.voxelWorld.setClipping('x', val, true);
        }
      });
    }

    if (clipY) {
      clipY.addEventListener('input', (e) => {
        const val = parseFloat(e.target.value);
        if (val >= parseFloat(clipY.max)) {
          clipYVal.textContent = 'OFF';
          this.voxelWorld.setClipping('y', null, true);
        } else {
          clipYVal.textContent = `${val.toFixed(1)}B`;
          this.voxelWorld.setClipping('y', val, true);
        }
      });
    }

    if (clipZ) {
      clipZ.addEventListener('input', (e) => {
        const val = parseFloat(e.target.value);
        if (val >= parseFloat(clipZ.max)) {
          clipZVal.textContent = 'OFF';
          this.voxelWorld.setClipping('z', null, true);
        } else {
          clipZVal.textContent = `${val >= 0 ? '+' : ''}${val.toFixed(1)}B`;
          this.voxelWorld.setClipping('z', val, true);
        }
      });
    }

    if (btnResetClip) {
      btnResetClip.addEventListener('click', () => {
        if (clipX) clipX.value = clipX.max;
        if (clipY) clipY.value = clipY.max;
        if (clipZ) clipZ.value = clipZ.max;
        if (clipXVal) clipXVal.textContent = 'OFF';
        if (clipYVal) clipYVal.textContent = 'OFF';
        if (clipZVal) clipZVal.textContent = 'OFF';
        this.voxelWorld.setClipping('all', null, clippingActive);
      });
    }

    // Live Block Deselect button
    const btnDeselect = document.getElementById('btn-deselect-block');
    if (btnDeselect) {
      btnDeselect.addEventListener('click', () => {
        this.voxelWorld.deselectBlock();
      });
    }

    // Step Search Bar
    const searchInput = document.getElementById('step-search-input');
    if (searchInput) {
      searchInput.addEventListener('input', (e) => {
        const query = e.target.value.trim().toLowerCase();
        if (!query) {
          searchInput.style.borderColor = 'var(--line)';
          return;
        }

        if (!this.modelData || !this.modelData.steps) return;

        const matchIdx = this.modelData.steps.findIndex((step) => {
          if (step.step.toLowerCase() === query || step.step.toLowerCase().endsWith('.' + query)) return true;
          if (step.title.toLowerCase().includes(query)) return true;
          if (step.subsystem && step.subsystem.toLowerCase().includes(query)) return true;
          if (step.level && step.level.toLowerCase().includes(query)) return true;
          if (step.instructions && step.instructions.some((i) => i.toLowerCase().includes(query))) return true;
          if (step.partsTray && step.partsTray.some((p) => p.type.toLowerCase().includes(query))) return true;
          return false;
        });

        if (matchIdx !== -1) {
          searchInput.style.borderColor = 'var(--amber)';
          this.updateStep(matchIdx);
        } else {
          searchInput.style.borderColor = '#e74c3c';
        }
      });
    }

    // Explode layer slider
    const explodeSlider = document.getElementById('explode-slider');
    if (explodeSlider) {
      explodeSlider.addEventListener('input', (e) => {
        const factor = parseFloat(e.target.value);
        this.voxelWorld.setExplode(factor);
        document.getElementById('explode-val').textContent = `${Math.round(factor * 100)}%`;
      });
    }

    // 2D Slice Drawer Toggle
    const sliceDrawer = document.getElementById('slice-drawer');
    document.getElementById('btn-toggle-slice').addEventListener('click', () => {
      const isOpen = sliceDrawer.classList.toggle('collapsed') === false;
      document.getElementById('btn-toggle-slice').setAttribute('aria-pressed', String(isOpen));
      this.updateSliceView();
    });
    document.getElementById('btn-close-slice').addEventListener('click', () => {
      sliceDrawer.classList.add('collapsed');
      document.getElementById('btn-toggle-slice').setAttribute('aria-pressed', 'false');
    });

    // 2D Slice Block Node Click -> 3D Selection
    const svgWrap = document.getElementById('slice-svg-container');
    if (svgWrap) {
      svgWrap.addEventListener('click', (e) => {
        const node = e.target.closest('.slice-block-node');
        if (node && node.dataset.pos) {
          const parts = node.dataset.pos.split(',').map(Number);
          if (parts.length === 3) {
            this.voxelWorld.selectBlockByPos(parts[0], parts[1], parts[2]);
          }
        }
      });
    }

    // Level selector buttons in slice drawer
    document.querySelectorAll('.level-btn').forEach((btn) => {
      btn.addEventListener('click', () => {
        document.querySelectorAll('.level-btn').forEach((b) => b.classList.remove('active'));
        btn.classList.add('active');
        document.querySelectorAll('.level-btn').forEach((b) => b.setAttribute('aria-pressed', String(b === btn)));
        this.activeLevel = btn.dataset.lvl;
        this.updateSliceView();
      });
    });

    // Collaboration Log Export
    document.getElementById('btn-export-log').addEventListener('click', () => {
      this.exportCollaborationLog();
    });

    // Exporters
    document.getElementById('btn-export-png').addEventListener('click', () => {
      const dataUrl = this.voxelWorld.captureSnapshot();
      SnapshotApi.downloadPng(dataUrl, `${this.modelData.id}-step-${this.currentStepIdx}.png`);
    });

    document.getElementById('btn-export-ascii').addEventListener('click', () => {
      const ascii = Slicer2D.generateAsciiSlice(this.modelData, this.activeLevel, this.currentStepIdx);
      SnapshotApi.downloadFile(`${this.modelData.id}-slice-${this.activeLevel}.txt`, ascii);
    });

    document.getElementById('btn-export-svg').addEventListener('click', () => {
      const svg = Slicer2D.generateSvgSlice(this.modelData, this.activeLevel, this.currentStepIdx);
      SnapshotApi.downloadFile(`${this.modelData.id}-slice-${this.activeLevel}.svg`, svg, 'image/svg+xml');
    });

    const btnAsciiTab = document.getElementById('btn-export-ascii-tab');
    if (btnAsciiTab) {
      btnAsciiTab.addEventListener('click', () => {
        const ascii = Slicer2D.generateAsciiSlice(this.modelData, this.activeLevel, this.currentStepIdx);
        SnapshotApi.downloadFile(`${this.modelData.id}-slice-${this.activeLevel}.txt`, ascii);
      });
    }

    const btnSvgTab = document.getElementById('btn-export-svg-tab');
    if (btnSvgTab) {
      btnSvgTab.addEventListener('click', () => {
        const svg = Slicer2D.generateSvgSlice(this.modelData, this.activeLevel, this.currentStepIdx);
        SnapshotApi.downloadFile(`${this.modelData.id}-slice-${this.activeLevel}.svg`, svg, 'image/svg+xml');
      });
    }

    document.getElementById('btn-export-sbc').addEventListener('click', () => {
      const sbc = SnapshotApi.generateSbcXml(this.modelData, this.currentStepIdx);
      SnapshotApi.downloadFile(`${this.modelData.id}-step-${this.modelData.steps[this.currentStepIdx].step}-draft.xml`, sbc, 'application/xml');
    });

    document.getElementById('btn-export-json').addEventListener('click', () => {
      const json = JSON.stringify(this.modelData, null, 2);
      SnapshotApi.downloadFile(`${this.modelData.id}.json`, json, 'application/json');
    });

    // Keyboard Shortcuts
    window.addEventListener('keydown', (e) => {
      const tag = document.activeElement ? document.activeElement.tagName.toLowerCase() : '';
      if (tag === 'input' || tag === 'textarea' || tag === 'select') {
        if (e.key === 'Escape') {
          document.activeElement.blur();
        }
        return;
      }

      if (e.key === 'ArrowLeft') {
        e.preventDefault();
        if (this.currentStepIdx > 0) this.updateStep(this.currentStepIdx - 1);
      } else if (e.key === 'ArrowRight') {
        e.preventDefault();
        if (this.modelData && this.currentStepIdx < this.modelData.steps.length - 1) {
          this.updateStep(this.currentStepIdx + 1);
        }
      } else if (e.key === 't' || e.key === 'T') {
        e.preventDefault();
        btnOrbit?.click();
      } else if (e.key === 'c' || e.key === 'C') {
        e.preventDefault();
        btnClip?.click();
      } else if (e.key === 's' || e.key === 'S') {
        e.preventDefault();
        document.getElementById('btn-toggle-slice')?.click();
      } else if (e.key === 'Escape') {
        this.voxelWorld.deselectBlock();
        sliceDrawer?.classList.add('collapsed');
        document.getElementById('btn-toggle-slice').setAttribute('aria-pressed', 'false');
      } else if (['1', '2', '3', '4'].includes(e.key)) {
        const tabMap = { '1': 'build', '2': 'instructions', '3': 'collab', '4': 'export' };
        const targetTab = document.querySelector(`.rail-tab[data-tab="${tabMap[e.key]}"]`);
        targetTab?.click();
      }
    });
  }

  bindPwaEvents() {
    const installButton = document.getElementById('btn-install-app');
    const updateButton = document.getElementById('btn-update-app');

    window.addEventListener('beforeinstallprompt', (event) => {
      event.preventDefault();
      this.installPromptEvent = event;
      if (!this.isAppInstalled() && installButton) installButton.hidden = false;
    });

    window.addEventListener('appinstalled', () => {
      this.installPromptEvent = null;
      if (installButton) installButton.hidden = true;
      this.setPwaStatus('App installed', 'ready');
    });

    installButton?.addEventListener('click', async () => {
      const promptEvent = this.installPromptEvent;
      if (!promptEvent) return;

      this.installPromptEvent = null;
      installButton.hidden = true;
      installButton.disabled = true;
      try {
        await promptEvent.prompt();
        const choice = await promptEvent.userChoice;
        this.setPwaStatus(
          choice.outcome === 'accepted' ? 'Install accepted' : 'Install dismissed',
          choice.outcome === 'accepted' ? 'ready' : 'online'
        );
      } catch (error) {
        console.warn('Unable to start app installation:', error);
        this.setPwaStatus('App installation failed', 'error');
      } finally {
        installButton.disabled = false;
      }
    });

    updateButton?.addEventListener('click', () => {
      const waitingWorker = this.serviceWorkerRegistration?.waiting;
      if (!waitingWorker) {
        this.setPwaStatus('No app update is waiting', 'ready');
        return;
      }

      this.applyingServiceWorkerUpdate = true;
      updateButton.disabled = true;
      updateButton.textContent = 'Updating…';
      waitingWorker.postMessage({ type: 'SKIP_WAITING' });
    });

    window.addEventListener('online', () => this.updatePwaStatus());
    window.addEventListener('offline', () => this.updatePwaStatus());

    if ('serviceWorker' in navigator) {
      navigator.serviceWorker.addEventListener('controllerchange', () => {
        this.updatePwaStatus();
        if (this.applyingServiceWorkerUpdate && !this.didReloadForServiceWorkerUpdate) {
          this.didReloadForServiceWorkerUpdate = true;
          window.location.reload();
        }
      });
    }

    this.updatePwaStatus();
  }

  isAppInstalled() {
    return Boolean(
      (window.matchMedia && window.matchMedia('(display-mode: standalone)').matches) ||
      navigator.standalone === true
    );
  }

  setPwaStatus(message, state) {
    const status = document.getElementById('pwa-status');
    if (!status) return;
    status.textContent = message;
    status.dataset.state = state;
  }

  updatePwaStatus() {
    const serviceWorkerAvailable = 'serviceWorker' in navigator;
    const controlled = serviceWorkerAvailable && Boolean(navigator.serviceWorker.controller);

    if (!navigator.onLine) {
      this.setPwaStatus(
        controlled ? 'Offline · cached viewer available' : 'Offline · cache not ready',
        controlled ? 'offline' : 'error'
      );
    } else if (!serviceWorkerAvailable) {
      this.setPwaStatus('Offline support unavailable', 'error');
    } else if (controlled) {
      this.setPwaStatus('Online · offline viewer ready', 'ready');
    } else {
      this.setPwaStatus('Online · preparing offline access', 'pending');
    }
  }

  async registerServiceWorker() {
    if (!('serviceWorker' in navigator)) {
      this.updatePwaStatus();
      return;
    }

    try {
      const registration = await navigator.serviceWorker.register('./sw.js');
      this.serviceWorkerRegistration = registration;
      registration.addEventListener('updatefound', () => {
        const installingWorker = registration.installing;
        if (!installingWorker) return;
        installingWorker.addEventListener('statechange', () => {
          if (installingWorker.state !== 'installed') return;
          if (navigator.serviceWorker.controller) this.showServiceWorkerUpdate();
          else this.updatePwaStatus();
        });
      });

      if (registration.waiting && navigator.serviceWorker.controller) {
        this.showServiceWorkerUpdate();
      }
      this.updatePwaStatus();
      registration.update().catch((error) => {
        console.warn('Service worker update check failed:', error);
      });
    } catch (error) {
      console.warn('Service worker registration failed:', error);
      this.setPwaStatus('Offline caching unavailable', 'error');
      const status = document.getElementById('load-status');
      if (status && !status.textContent) {
        status.textContent = 'Offline caching is unavailable in this browser session.';
      }
    }
  }

  showServiceWorkerUpdate() {
    const updateButton = document.getElementById('btn-update-app');
    if (updateButton) updateButton.hidden = false;
    this.setPwaStatus('Update available · refresh when ready', 'update');
  }
}

window.addEventListener('DOMContentLoaded', () => {
  new AppController();
});
