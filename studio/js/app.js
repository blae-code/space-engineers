import { VoxelWorld } from './VoxelWorld.js';
import { Slicer2D } from './Slicer2D.js';
import { SnapshotApi } from './SnapshotApi.js';

class AppController {
  constructor() {
    this.currentStepIdx = 0;
    this.modelData = null;
    this.activeLevel = 'L2';
    this.currentFile = 'mastodon.json';
    this.init();
  }

  async init() {
    this.canvasContainer = document.getElementById('canvas-container');
    this.voxelWorld = new VoxelWorld(this.canvasContainer, {
      onBlockSelected: (data) => this.handleBlockSelected(data)
    });

    this.bindEvents();
    this.registerServiceWorker();

    await this.loadBlueprint('./data/mastodon.json');
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
    try {
      const res = await fetch(url);
      this.modelData = await res.json();
      this.voxelWorld.loadModel(this.modelData);

      this.initStepSlider();
      this.initSubsystemToggles();
      this.updateClippingBounds();
      this.updateStep(0);
      this.updateSliceView();
      this.updateCollabStats();
    } catch (err) {
      console.error('Failed to load blueprint data:', err);
      document.getElementById('hud-step-desc').textContent = 'Error loading blueprint JSON';
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
        this.voxelWorld.toggleSubsystem(sub, isActive);
      };
    });
  }

  getStorageKey(prefix) {
    return `se3d_${prefix}_${this.modelData?.id || 'default'}`;
  }

  getStoredChecks() {
    try {
      const val = localStorage.getItem(this.getStorageKey('checks'));
      return val ? JSON.parse(val) : {};
    } catch (e) {
      return {};
    }
  }

  setStoredCheck(checkKey, isChecked) {
    try {
      const checks = this.getStoredChecks();
      checks[checkKey] = isChecked;
      localStorage.setItem(this.getStorageKey('checks'), JSON.stringify(checks));
      this.updateCollabStats();
    } catch (e) {}
  }

  getStoredNote(stepId) {
    try {
      return localStorage.getItem(`${this.getStorageKey('note')}_${stepId}`) || '';
    } catch (e) {
      return '';
    }
  }

  setStoredNote(stepId, noteText) {
    try {
      localStorage.setItem(`${this.getStorageKey('note')}_${stepId}`, noteText);
      this.updateCollabStats();
    } catch (e) {}
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
    this.currentStepIdx = idx;
    document.getElementById('step-slider').value = idx;
    this.voxelWorld.setStep(idx);

    const step = this.modelData.steps[idx];
    if (!step) return;

    // Update HUD
    document.getElementById('hud-step-badge').textContent = `STEP ${step.step}`;
    document.getElementById('hud-step-desc').textContent = `${step.title} (${step.level})`;
    document.getElementById('hud-phase-badge').textContent = `PHASE ${step.phase}`;

    // Update Inspector Sidebar Header
    document.getElementById('meta-model-name').textContent = this.modelData.name;
    document.getElementById('meta-step-title').textContent = step.title;
    document.getElementById('meta-phase').textContent = `PHASE ${step.phase}`;
    document.getElementById('meta-level').textContent = `LEVEL ${step.level}`;
    document.getElementById('meta-grid-metric').textContent = `${this.modelData.gridSize} (${this.modelData.blockMeters} m)`;
    this.activeLevel = step.level.split('-')[0] || 'L2';

    // Calculate current cumulative stats
    let totalBlocks = 0;
    let totalMass = 0;
    for (let i = 0; i <= idx; i++) {
      this.modelData.steps[i].blocks.forEach(() => {
        totalBlocks++;
        totalMass += (this.modelData.gridSize === 'Large' ? 1.2 : 0.08);
      });
    }
    document.getElementById('meta-blocks-count').textContent = `${totalBlocks} / ${this.modelData.stats?.totalBlocks || 'TBD'}`;
    document.getElementById('meta-mass').textContent = `${totalMass.toFixed(1)} t`;

    // Update Parts Tray
    const partsBody = document.getElementById('parts-tray-body');
    partsBody.innerHTML = '';
    if (step.partsTray && step.partsTray.length > 0) {
      step.partsTray.forEach((p) => {
        const pal = this.modelData.palette?.find((item) => item.slot === Number(p.slot));
        const colorSwatch = pal
          ? `<span class="swatch-dot" style="display:inline-block; width:10px; height:10px; border-radius:50%; background:${pal.hex}; border:1px solid rgba(255,255,255,0.4); margin-right:5px; vertical-align:middle;"></span>`
          : '';
        const palName = pal ? `<span style="color:var(--muted); font-size:10px; display:block;">${pal.name}</span>` : '';
        const row = document.createElement('tr');
        row.innerHTML = `
          <td><strong>${p.type}</strong></td>
          <td class="mono">${p.qty}</td>
          <td class="mono" style="white-space:nowrap;">${colorSwatch}Slot ${p.slot}${palName}</td>
          <td style="font-size:11px;">${p.note || p.massPcu || ''}</td>
        `;
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
        item.innerHTML = `
          <input type="checkbox" ${isChecked ? 'checked' : ''}>
          <span>${c}</span>
        `;

        item.querySelector('input').addEventListener('change', (e) => {
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
    lines.push(`* **Total Blocks:** ${this.modelData.stats.totalBlocks}`);
    lines.push(`* **Dry Mass:** ${this.modelData.stats.dryMassTonnes} t`);
    lines.push(`* **PCU:** ${this.modelData.stats.pcu}\n`);
    lines.push(`---`);

    this.modelData.steps.forEach((step) => {
      lines.push(`\n### Step ${step.step} · ${step.title} (${step.level})`);
      lines.push(`* **Phase:** ${step.phase} · **Subsystem:** ${step.subsystem}`);

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
      this.currentFile = e.target.value;
      this.loadBlueprint(`./data/${e.target.value}`);
    });

    // Inspector Tab switching
    document.querySelectorAll('.rail-tab').forEach((tab) => {
      tab.addEventListener('click', () => {
        document.querySelectorAll('.rail-tab').forEach((t) => t.classList.remove('active'));
        tab.classList.add('active');

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
      saveTimeout = setTimeout(() => {
        const step = this.modelData.steps[this.currentStepIdx];
        if (step) {
          this.setStoredNote(step.step, e.target.value);
          notesStatus.textContent = 'Saved to localStorage';
        }
      }, 350);
    });

    document.getElementById('btn-clear-note').addEventListener('click', () => {
      const step = this.modelData.steps[this.currentStepIdx];
      if (step) {
        notesInput.value = '';
        this.setStoredNote(step.step, '');
        notesStatus.textContent = 'Note cleared';
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
        this.voxelWorld.setCameraView(btn.dataset.cam);
      });
    });

    // Render Mode buttons (Solid, Ghost, X-Ray, Wire)
    document.querySelectorAll('.mode-btn').forEach((btn) => {
      btn.addEventListener('click', () => {
        document.querySelectorAll('.mode-btn').forEach((b) => b.classList.remove('active'));
        btn.classList.add('active');
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
      sliceDrawer.classList.toggle('collapsed');
      this.updateSliceView();
    });
    document.getElementById('btn-close-slice').addEventListener('click', () => {
      sliceDrawer.classList.add('collapsed');
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
      SnapshotApi.downloadFile(`${this.modelData.id}-bp.sbc`, sbc, 'application/xml');
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
      } else if (['1', '2', '3', '4'].includes(e.key)) {
        const tabMap = { '1': 'build', '2': 'instructions', '3': 'collab', '4': 'export' };
        const targetTab = document.querySelector(`.rail-tab[data-tab="${tabMap[e.key]}"]`);
        targetTab?.click();
      }
    });
  }

  registerServiceWorker() {
    if ('serviceWorker' in navigator) {
      navigator.serviceWorker.register('./sw.js').catch((err) => {
        console.log('ServiceWorker registration skipped:', err);
      });
    }
  }
}

window.addEventListener('DOMContentLoaded', () => {
  new AppController();
});
