import * as THREE from 'three';
import { OrbitControls } from '../vendor/OrbitControls.js';
import { toThreeEulerRadians } from './Orientation.mjs';

export class VoxelWorld {
  constructor(canvasContainer, callbacks = {}, options = {}) {
    this.container = canvasContainer;
    this.callbacks = callbacks; // e.g. onBlockSelected, onBlockHovered
    // Test mode: frozen animation clock and no damping, so captures are deterministic.
    this.testMode = Boolean(options.testMode);

    this.blocks = [];
    this.currentStepIndex = 0;
    this.explodeFactor = 0;
    this.activeSubsystems = new Set(['structure', 'conveyor', 'power', 'propulsion', 'avionics', 'flightdeck', 'boom']);
    this.renderMode = 'standard'; // 'standard', 'ghost-future', 'x-ray', 'wireframe'
    this.cameraMode = 'iso';

    this.palette = [];
    this.paletteMaterials = [];
    this.paletteXRayMaterials = [];
    this.blockMeters = 2.5;
    this.modelCenter = new THREE.Vector3();
    this.modelRadius = 10;
    this.hologramMaterial = null;
    this.futureWireframeMaterial = null;

    // Interactive Raycasting
    this.raycaster = new THREE.Raycaster();
    this.pointer = new THREE.Vector2();
    this.selectedBlock = null;
    this.hoveredBlock = null;

    // Section Clipping Planes
    this.clippingEnabled = false;
    this.clipPlanes = [
      new THREE.Plane(new THREE.Vector3(-1, 0, 0), 1000), // X (Starboard -> Port)
      new THREE.Plane(new THREE.Vector3(0, -1, 0), 1000), // Y (Zenith -> Keel)
      new THREE.Plane(new THREE.Vector3(0, 0, 1), 1000)   // Z (Bow -> Stern)
    ];

    this.initScene();
    this.initMaterials();
    this.initSkybox();
    this.initHelpers();
    this.initRaycasting();

    this.animate = this.animate.bind(this);
    requestAnimationFrame(this.animate);
  }

  initScene() {
    this.scene = new THREE.Scene();
    this.scene.background = new THREE.Color(0x0a0e14);
    this.scene.fog = new THREE.FogExp2(0x0a0e14, 0.0035);

    const aspect = this.container.clientWidth / this.container.clientHeight;
    this.cameraPersp = new THREE.PerspectiveCamera(45, aspect, 0.5, 1200);
    this.cameraPersp.position.set(55, 35, 60);

    this.cameraOrtho = new THREE.OrthographicCamera(-50 * aspect, 50 * aspect, 50, -50, 0.1, 1500);
    this.cameraOrtho.position.set(0, 120, 0);
    this.cameraOrtho.lookAt(0, 5, 0);

    this.camera = this.cameraPersp;

    this.renderer = new THREE.WebGLRenderer({ antialias: true, alpha: false, preserveDrawingBuffer: true });
    this.renderer.setSize(this.container.clientWidth, this.container.clientHeight);
    this.renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2));
    this.renderer.shadowMap.enabled = true;
    this.renderer.shadowMap.type = THREE.PCFSoftShadowMap;
    this.renderer.localClippingEnabled = true;
    this.container.appendChild(this.renderer.domElement);

    this.controls = new OrbitControls(this.camera, this.renderer.domElement);
    this.controls.enableDamping = !this.testMode;
    this.controls.dampingFactor = 0.05;
    this.controls.target.copy(this.modelCenter);
    this.controls.maxPolarAngle = Math.PI / 2 + 0.08;

    // Advanced Lighting Suite (Sun + Planetary Horizon Bounce + Tactical Rim)
    const ambientLight = new THREE.AmbientLight(0xdde6f0, 0.65);
    this.scene.add(ambientLight);

    const sunLight = new THREE.DirectionalLight(0xfff5e6, 1.4);
    sunLight.position.set(70, 120, 80);
    sunLight.castShadow = true;
    sunLight.shadow.mapSize.width = 2048;
    sunLight.shadow.mapSize.height = 2048;
    sunLight.shadow.bias = -0.0005;
    sunLight.shadow.camera.near = 10;
    sunLight.shadow.camera.far = 300;
    sunLight.shadow.camera.left = -90;
    sunLight.shadow.camera.right = 90;
    sunLight.shadow.camera.top = 90;
    sunLight.shadow.camera.bottom = -90;
    this.scene.add(sunLight);

    const planetBounceLight = new THREE.DirectionalLight(0x23384a, 0.6);
    planetBounceLight.position.set(0, -60, 0);
    this.scene.add(planetBounceLight);

    const cyanRimLight = new THREE.DirectionalLight(0x1abc9c, 0.45);
    cyanRimLight.position.set(-80, 50, -80);
    this.scene.add(cyanRimLight);

    this.rootGroup = new THREE.Group();
    this.scene.add(this.rootGroup);

    this.comMarker = this.createCoMMarker();
    this.scene.add(this.comMarker);

    this.selectionBox = this.createSelectionBox();
    this.scene.add(this.selectionBox);

    window.addEventListener('resize', () => this.onResize());
  }

  initSkybox() {
    // 1,500 Starfield Particles
    const starCount = 1500;
    const starGeo = new THREE.BufferGeometry();
    const starPositions = new Float32Array(starCount * 3);

    // Seeded (mulberry32) so the sky is identical on every load and in visual tests.
    let seed = 0x5e3d;
    const random = () => {
      seed = (seed + 0x6d2b79f5) | 0;
      let t = Math.imul(seed ^ (seed >>> 15), 1 | seed);
      t = (t + Math.imul(t ^ (t >>> 7), 61 | t)) ^ t;
      return ((t ^ (t >>> 14)) >>> 0) / 4294967296;
    };

    for (let i = 0; i < starCount * 3; i += 3) {
      starPositions[i] = (random() - 0.5) * 1600;
      starPositions[i + 1] = random() * 600 + 40;
      starPositions[i + 2] = (random() - 0.5) * 1600;
    }

    starGeo.setAttribute('position', new THREE.BufferAttribute(starPositions, 3));
    const starMat = new THREE.PointsMaterial({ color: 0xffffff, size: 1.8, transparent: true, opacity: 0.85 });
    const starField = new THREE.Points(starGeo, starMat);
    this.scene.add(starField);

    // Planetary Atmosphere Haze Ring on Horizon
    const ringGeo = new THREE.RingGeometry(180, 240, 64);
    const ringMat = new THREE.MeshBasicMaterial({ color: 0x14202d, side: THREE.DoubleSide, transparent: true, opacity: 0.4 });
    const ring = new THREE.Mesh(ringGeo, ringMat);
    ring.rotation.x = Math.PI / 2;
    ring.position.y = -0.2;
    this.scene.add(ring);
  }

  initMaterials() {
    // Pulsing Projector Hologram Shader Material (SE Cyan Blueprint Aesthetic)
    this.hologramMaterial = new THREE.MeshStandardMaterial({
      color: 0x1abc9c,
      emissive: 0x1abc9c,
      emissiveIntensity: 0.8,
      transparent: true,
      opacity: 0.7,
      roughness: 0.2,
      metalness: 0.1,
      wireframe: false,
      clippingPlanes: this.clipPlanes
    });

    this.futureWireframeMaterial = new THREE.MeshBasicMaterial({
      color: 0x425667,
      wireframe: true,
      transparent: true,
      opacity: 0.18,
      clippingPlanes: this.clipPlanes
    });

    this.activeWireframeMaterial = new THREE.MeshBasicMaterial({
      color: 0x1abc9c,
      wireframe: true,
      transparent: true,
      opacity: 0.9,
      clippingPlanes: this.clipPlanes
    });

    this.selectionMaterial = new THREE.LineBasicMaterial({
      color: 0xf1c40f,
      linewidth: 3,
      depthTest: false
    });
    this.orientationFaceMaterial = new THREE.MeshBasicMaterial({ color: 0x48c8ff });
    this.orientationFlowMaterial = new THREE.MeshBasicMaterial({ color: 0x1abc9c });
    this.orientationShaftMaterial = new THREE.MeshStandardMaterial({
      color: 0xe6a100,
      metalness: 0.65,
      roughness: 0.35
    });
    this.orientationHubMaterial = new THREE.MeshStandardMaterial({
      color: 0xe6a100,
      metalness: 0.7,
      roughness: 0.3
    });
    this.orientationAxisMaterial = new THREE.MeshStandardMaterial({
      color: 0x48c8ff,
      metalness: 0.55,
      roughness: 0.35
    });
    this.edgeMaterial = new THREE.LineBasicMaterial({
      color: 0x000000,
      linewidth: 1.2,
      clippingPlanes: this.clipPlanes
    });
  }

  setPalette(palette) {
    this.paletteMaterials.forEach((material) => material.dispose());
    this.paletteXRayMaterials.forEach((material) => material.dispose());
    this.palette = palette;
    this.paletteMaterials = palette.map((p) => {
      return new THREE.MeshStandardMaterial({
        color: new THREE.Color(p.hex),
        roughness: 0.42,
        metalness: 0.38,
        clippingPlanes: this.clipPlanes
      });
    });

    this.paletteXRayMaterials = palette.map((p) => {
      return new THREE.MeshStandardMaterial({
        color: new THREE.Color(p.hex),
        roughness: 0.3,
        metalness: 0.5,
        transparent: true,
        opacity: p.role.includes('Hull') || p.role.includes('Armor') ? 0.22 : 0.95,
        clippingPlanes: this.clipPlanes
      });
    });
  }

  initHelpers() {
    this.createGridHelper(100);
    const arrowDir = new THREE.Vector3(0, 0, -1);
    this.arrowHelper = new THREE.ArrowHelper(arrowDir, new THREE.Vector3(0, 0.15, 0), 14, 0xe6a100, 4, 2.5);
    this.scene.add(this.arrowHelper);
  }

  createGridHelper(blocksAcross) {
    if (this.gridHelper) {
      this.scene.remove(this.gridHelper);
      this.gridHelper.geometry.dispose();
      const materials = Array.isArray(this.gridHelper.material)
        ? this.gridHelper.material
        : [this.gridHelper.material];
      materials.forEach((material) => material.dispose());
    }
    this.gridHelper = new THREE.GridHelper(
      blocksAcross * this.blockMeters,
      blocksAcross,
      0xe6a100,
      0x1c2430
    );
    this.scene.add(this.gridHelper);
  }

  createCoMMarker() {
    const group = new THREE.Group();
    const sphere = new THREE.Mesh(
      new THREE.SphereGeometry(1.0, 16, 16),
      new THREE.MeshStandardMaterial({ color: 0xf1c40f, emissive: 0xf1c40f, emissiveIntensity: 0.5, metalness: 0.8, roughness: 0.2 })
    );
    group.add(sphere);

    const axes = new THREE.AxesHelper(4.0);
    group.add(axes);
    group.position.set(0, 5, 0);
    return group;
  }

  createSelectionBox() {
    const geom = new THREE.BoxGeometry(1, 1, 1);
    const edges = new THREE.EdgesGeometry(geom);
    const box = new THREE.LineSegments(edges, this.selectionMaterial);
    box.visible = false;
    box.renderOrder = 999;
    return box;
  }

  initRaycasting() {
    const dom = this.renderer.domElement;
    let downX = 0;
    let downY = 0;

    dom.addEventListener('pointerdown', (e) => {
      downX = e.clientX;
      downY = e.clientY;
    });

    dom.addEventListener('pointerup', (e) => {
      const dist = Math.hypot(e.clientX - downX, e.clientY - downY);
      if (dist > 6) return; // Ignore drag operations (orbit / pan)

      const rect = dom.getBoundingClientRect();
      this.pointer.x = ((e.clientX - rect.left) / rect.width) * 2 - 1;
      this.pointer.y = -((e.clientY - rect.top) / rect.height) * 2 + 1;

      this.raycaster.setFromCamera(this.pointer, this.camera);
      const visibleMeshes = this.blocks.filter((m) => m.visible);
      const intersects = this.raycaster.intersectObjects(visibleMeshes, true);

      if (intersects.length > 0) {
        let hitMesh = intersects[0].object;
        while (hitMesh.parent && hitMesh.parent !== this.rootGroup) {
          hitMesh = hitMesh.parent;
        }

        if (hitMesh.userData && hitMesh.userData.pos) {
          this.selectBlock(hitMesh);
        }
      } else {
        this.deselectBlock();
      }
    });
  }

  selectBlock(mesh) {
    this.selectedBlock = mesh;
    const bbox = new THREE.Box3().setFromObject(mesh);
    const size = new THREE.Vector3();
    const center = new THREE.Vector3();
    bbox.getSize(size);
    bbox.getCenter(center);

    this.selectionBox.position.copy(center);
    this.selectionBox.scale.set(
      Math.max(size.x, this.blockMeters * 1.01),
      Math.max(size.y, this.blockMeters * 1.01),
      Math.max(size.z, this.blockMeters * 1.01)
    );
    this.selectionBox.visible = true;

    if (this.callbacks.onBlockSelected) {
      this.callbacks.onBlockSelected(mesh.userData);
    }
  }

  selectBlockByPos(x, y, z) {
    const target = this.blocks.find((m) => {
      const p = m.userData.pos;
      return p[0] === x && p[1] === y && p[2] === z;
    });
    if (target) {
      this.selectBlock(target);
      return target;
    }
    return null;
  }

  deselectBlock() {
    this.selectedBlock = null;
    this.selectionBox.visible = false;
    if (this.callbacks.onBlockSelected) {
      this.callbacks.onBlockSelected(null);
    }
  }

  // Grid-scaled procedural preview geometry, not game-rendered block meshes.
  getGeometryForShape(shape, subtype) {
    const B = this.blockMeters;

    if (subtype === 'NarrowLadderBlock') {
      return new THREE.BoxGeometry(B * 0.08, B * 0.08, B);
    }

    switch (shape) {
      case 'half':
        return new THREE.BoxGeometry(B, B * 0.5, B);

      case 'halfslope': {
        const shape2d = new THREE.Shape();
        shape2d.moveTo(-B * 0.5, -B * 0.25);
        shape2d.lineTo(B * 0.5, -B * 0.25);
        shape2d.lineTo(B * 0.5, B * 0.25);
        shape2d.closePath();
        const extrudeSettings = { depth: B, bevelEnabled: false };
        const geom = new THREE.ExtrudeGeometry(shape2d, extrudeSettings);
        geom.center();
        return geom;
      }

      case 'slope2x1': {
        const shape2d = new THREE.Shape();
        shape2d.moveTo(0, 0);
        shape2d.lineTo(B, 0);
        shape2d.lineTo(0, B * 0.5);
        shape2d.closePath();
        const extrudeSettings = { depth: B, bevelEnabled: false };
        const geom = new THREE.ExtrudeGeometry(shape2d, extrudeSettings);
        geom.center();
        return geom;
      }

      case 'conveyor': {
        // Multi-port conveyor junction: central cubic core + 6 port coamings
        return new THREE.BoxGeometry(B * 0.88, B * 0.88, B * 0.88);
      }

      case 'wheel5x5': {
        // 5x5 Wheel: Large 12.5 m diameter wheel with tire tread
        const geom = new THREE.CylinderGeometry(B * 2.5, B * 2.5, B * 0.95, 32);
        geom.rotateZ(Math.PI / 2);
        return geom;
      }

      case 'rotor':
      case 'hinge': {
        const geom = new THREE.CylinderGeometry(B * 0.42, B * 0.48, B * 0.9, 20);
        return geom;
      }

      case 'cargo':
        return new THREE.BoxGeometry(B * 2.92, B * 2.92, B * 2.92);

      case 'cargo_small':
        return new THREE.BoxGeometry(B * 0.95, B * 0.95, B * 0.95);

      case 'refinery':
        return new THREE.BoxGeometry(B * 0.95, B * 1.95, B * 0.95);

      case 'assembler':
        return new THREE.BoxGeometry(B * 0.95, B * 1.95, B * 0.95);

      case 'generator':
      case 'engine':
        return new THREE.BoxGeometry(B * 0.95, B * 0.95, B * 1.9);

      case 'solar':
        return new THREE.BoxGeometry(B * 0.98, B * 0.1, B * 0.98);

      case 'catwalk':
        return new THREE.BoxGeometry(B * 0.98, B * 0.08, B * 0.98);

      case 'welder':
        return new THREE.BoxGeometry(B * 0.92, B * 0.92, B * 0.92);

      case 'piston': {
        const geom = new THREE.CylinderGeometry(B * 0.35, B * 0.35, B * 0.95, 16);
        return geom;
      }

      case 'reactor': {
        const geom = new THREE.CylinderGeometry(B * 0.46, B * 0.46, B * 0.95, 24);
        return geom;
      }

      case 'battery':
        return new THREE.BoxGeometry(B * 0.96, B * 0.96, B * 0.96);

      case 'tank': {
        const geom = new THREE.CylinderGeometry(B * 0.9, B * 0.9, B * 1.8, 24);
        return geom;
      }

      case 'turret': {
        const geom = new THREE.CylinderGeometry(B * 0.85, B * 0.95, B * 0.45, 20);
        return geom;
      }

      case 'cockpit':
        return new THREE.BoxGeometry(B * 0.95, B * 0.95, B * 0.95);

      case 'cube':
      default:
        return new THREE.BoxGeometry(B, B, B);
    }
  }

  addOrientationDetails(mesh, block, blockMeters) {
    const features = Array.isArray(block.orientationFeature)
      ? block.orientationFeature
      : [block.orientationFeature];
    features.forEach((feature) => {
      if (feature === 'forward-face' || feature === 'conveyor-flow' || feature === 'top-face') {
        const flow = feature === 'conveyor-flow';
        const top = feature === 'top-face';
        const direction = top ? new THREE.Vector3(0, 1, 0) : new THREE.Vector3(0, 0, -1);
        const marker = new THREE.Mesh(
          new THREE.ConeGeometry(blockMeters * 0.12, blockMeters * 0.28, 12),
          flow ? this.orientationFlowMaterial : this.orientationFaceMaterial
        );
        marker.position.copy(direction.clone().multiplyScalar(blockMeters * 0.36));
        marker.quaternion.setFromUnitVectors(new THREE.Vector3(0, 1, 0), direction);
        mesh.add(marker);
      } else if (feature === 'piston-shaft') {
        const shaft = new THREE.Mesh(
          new THREE.CylinderGeometry(blockMeters * 0.14, blockMeters * 0.14, blockMeters * 0.8, 12),
          this.orientationShaftMaterial
        );
        shaft.position.y = blockMeters * 0.65;
        mesh.add(shaft);
      } else if (feature === 'wheel-hub') {
        const side = block.subtype === 'Suspension5x5Left' ? -1 : 1;
        const hub = new THREE.Mesh(
          new THREE.CylinderGeometry(blockMeters * 0.18, blockMeters * 0.18, blockMeters * 0.08, 16),
          this.orientationHubMaterial
        );
        hub.rotation.z = Math.PI / 2;
        hub.position.x = side * blockMeters * 0.49;
        mesh.add(hub);
      } else if (feature === 'axis') {
        const capGeometry = new THREE.CylinderGeometry(blockMeters * 0.24, blockMeters * 0.24, blockMeters * 0.06, 16);
        [-1, 1].forEach((side) => {
          const cap = new THREE.Mesh(capGeometry, this.orientationAxisMaterial);
          cap.position.y = side * blockMeters * 0.48;
          mesh.add(cap);
        });
      } else if (feature === 'dynamic-turret') {
        const head = new THREE.Mesh(
          new THREE.SphereGeometry(blockMeters * 0.26, 12, 8),
          this.orientationAxisMaterial
        );
        head.position.y = blockMeters * 0.32;
        const barrel = new THREE.Mesh(
          new THREE.CylinderGeometry(blockMeters * 0.08, blockMeters * 0.08, blockMeters * 0.75, 10),
          this.orientationHubMaterial
        );
        barrel.rotation.x = Math.PI / 2;
        barrel.position.set(0, blockMeters * 0.32, -blockMeters * 0.42);
        mesh.add(head, barrel);
      } else if (feature === 'ladder') {
        const railGeometry = new THREE.BoxGeometry(blockMeters * 0.08, blockMeters * 0.08, blockMeters);
        [-1, 1].forEach((side) => {
          const rail = new THREE.Mesh(railGeometry, this.orientationShaftMaterial);
          rail.position.x = side * blockMeters * 0.34;
          mesh.add(rail);
        });
        const rungGeometry = new THREE.BoxGeometry(blockMeters * 0.76, blockMeters * 0.07, blockMeters * 0.08);
        for (let rungIndex = -2; rungIndex <= 2; rungIndex++) {
          const rung = new THREE.Mesh(rungGeometry, this.orientationHubMaterial);
          rung.position.z = rungIndex * blockMeters * 0.19;
          mesh.add(rung);
        }
      }
    });
  }

  loadModel(data) {
    this.modelData = data;
    this.blockMeters = data.blockMeters;
    this.comMarker.scale.setScalar(this.blockMeters * 0.35);
    this.setPalette(data.palette);

    while (this.rootGroup.children.length > 0) {
      const child = this.rootGroup.children[0];
      child.traverse((node) => {
        if (node.geometry) node.geometry.dispose();
      });
      this.rootGroup.remove(child);
    }
    this.blocks = [];
    this.deselectBlock();

    const B = this.blockMeters;

    data.steps.forEach((step, stepIdx) => {
      step.blocks.forEach((b) => {
        const geom = this.getGeometryForShape(b.shape, b.subtype);
        const slotIdx = Math.min(Math.max(b.paletteSlot || 0, 0), this.paletteMaterials.length - 1);
        const mat = this.paletteMaterials[slotIdx];

        const mesh = new THREE.Mesh(geom, mat);
        mesh.castShadow = true;
        mesh.receiveShadow = true;

        // Space Engineers Coordinates: X (East/Starboard), Y (Zenith/Up), Z (Bow/North)
        const xPos = b.pos[0] * B;
        const yPos = (b.pos[1] - 0.5) * B;
        const zPos = -b.pos[2] * B;

        mesh.position.set(xPos, yPos, zPos);
        mesh.rotation.order = 'XYZ';
        mesh.rotation.set(...toThreeEulerRadians(b.rot || [0, 0, 0]));
        if (b.scale) {
          mesh.scale.set(b.scale[0], b.scale[1], b.scale[2]);
        }
        this.addOrientationDetails(mesh, b, B);

        const edges = new THREE.EdgesGeometry(geom, 25);
        const line = new THREE.LineSegments(edges, this.edgeMaterial);
        mesh.add(line);

        mesh.userData = {
          stepIndex: stepIdx,
          stepId: step.step,
          stepTitle: step.title,
          phase: step.phase,
          pos: b.pos,
          baseY: yPos,
          subsystem: b.subsystem || 'structure',
          name: b.name || `${b.subtype || b.shape} [${b.pos.join(', ')}]`,
          subtype: b.subtype || b.shape,
          paletteSlot: slotIdx,
          paletteName: data.palette[slotIdx]?.name || 'Standard',
          shape: b.shape,
          orientationStatus: b.orientationStatus || 'not-audited',
          orientationEvidence: b.orientationEvidence || '',
          orientationFeature: b.orientationFeature || '',
          rotationDegrees: b.rot || [0, 0, 0]
        };

        this.rootGroup.add(mesh);
        this.blocks.push(mesh);
      });
    });

    const bounds = data.bounds;
    const min = bounds.min;
    const max = bounds.max;
    this.modelCenter.set(
      (min[0] + max[0]) * 0.5 * B,
      ((min[1] + max[1]) * 0.5 - 0.5) * B,
      -(min[2] + max[2]) * 0.5 * B
    );
    this.modelRadius = Math.hypot(
      (max[0] - min[0] + 1) * B,
      (max[1] - min[1] + 1) * B,
      (max[2] - min[2] + 1) * B
    ) * 0.5;
    const gridBlocksAcross = Math.max(100, Math.ceil((this.modelRadius * 4) / B));
    this.createGridHelper(gridBlocksAcross);
    this.arrowHelper.position.set(this.modelCenter.x, 0.15, this.modelCenter.z + this.modelRadius);
    this.arrowHelper.setLength(Math.max(B * 4, this.modelRadius * 0.35));
    this.cameraPersp.far = Math.max(1200, this.modelRadius * 10);
    this.cameraPersp.updateProjectionMatrix();
    this.setCameraView(this.cameraMode);
    this.updateVisibility();
    this.computeCoM();
  }

  setStep(stepIndex) {
    this.currentStepIndex = stepIndex;
    this.updateVisibility();
    this.computeCoM();
  }

  setExplode(factor) {
    this.explodeFactor = factor;
    this.blocks.forEach((mesh) => {
      const level = mesh.userData.pos[1];
      const offset = (level - 1) * factor * this.blockMeters * 4;
      mesh.position.y = mesh.userData.baseY + offset;
    });

    if (this.selectedBlock) {
      this.selectionBox.position.copy(this.selectedBlock.position);
    }
    this.computeCoM();
  }

  setRenderMode(mode) {
    this.renderMode = mode;
    this.updateVisibility();
  }

  setClipping(axis, value, enabled = true) {
    this.clippingEnabled = enabled;
    const B = this.blockMeters;

    if (!enabled || (axis === 'all' && value === null)) {
      this.clipPlanes.forEach((p) => (p.constant = 1000));
      return;
    }

    if (axis === 'x') {
      this.clipPlanes[0].constant = (value === null) ? 1000 : value * B;
    } else if (axis === 'y') {
      this.clipPlanes[1].constant = (value === null) ? 1000 : value * B;
    } else if (axis === 'z') {
      this.clipPlanes[2].constant = (value === null) ? 1000 : value * B;
    }
  }

  toggleSubsystem(subsystem, isVisible) {
    if (isVisible) {
      this.activeSubsystems.add(subsystem);
    } else {
      this.activeSubsystems.delete(subsystem);
    }
    this.updateVisibility();
    this.computeCoM();
  }

  updateVisibility() {
    this.blocks.forEach((mesh) => {
      const stepIdx = mesh.userData.stepIndex;
      const sub = mesh.userData.subsystem;
      const subAllowed = this.activeSubsystems.has(sub);

      if (!subAllowed) {
        mesh.visible = false;
        return;
      }

      if (stepIdx < this.currentStepIndex) {
        mesh.visible = true;
        if (this.renderMode === 'x-ray') {
          mesh.material = this.paletteXRayMaterials[mesh.userData.paletteSlot];
        } else if (this.renderMode === 'wireframe') {
          mesh.material = this.futureWireframeMaterial;
        } else {
          mesh.material = this.paletteMaterials[mesh.userData.paletteSlot];
        }
      } else if (stepIdx === this.currentStepIndex) {
        // Active step: Projector Hologram (or active wireframe)
        mesh.visible = true;
        mesh.material = (this.renderMode === 'wireframe') ? this.activeWireframeMaterial : this.hologramMaterial;
      } else {
        // Future step
        if (this.renderMode === 'ghost-future') {
          mesh.visible = true;
          mesh.material = this.futureWireframeMaterial;
        } else {
          mesh.visible = false;
        }
      }
    });

    if (this.selectedBlock && !this.selectedBlock.visible) {
      this.deselectBlock();
    }
  }

  computeCoM() {
    let sumX = 0, sumY = 0, sumZ = 0, count = 0;

    this.blocks.forEach((mesh) => {
      if (mesh.visible && mesh.userData.stepIndex <= this.currentStepIndex) {
        sumX += mesh.position.x;
        sumY += mesh.position.y;
        sumZ += mesh.position.z;
        count++;
      }
    });

    if (count > 0) {
      this.comMarker.position.set(sumX / count, sumY / count, sumZ / count);
      this.comMarker.visible = true;
    } else {
      this.comMarker.visible = false;
    }
  }

  setCameraView(viewName) {
    this.cameraMode = viewName;
    const dist = Math.max(this.modelRadius * 3.2, this.blockMeters * 8);
    const aspect = this.container.clientWidth / Math.max(1, this.container.clientHeight);
    const halfHeight = Math.max(this.modelRadius * 1.15, this.blockMeters * 4);

    if (viewName === 'plan') {
      this.camera = this.cameraOrtho;
      this.cameraOrtho.left = -halfHeight * aspect;
      this.cameraOrtho.right = halfHeight * aspect;
      this.cameraOrtho.top = halfHeight;
      this.cameraOrtho.bottom = -halfHeight;
      this.cameraOrtho.up.set(0, 0, -1);
      this.cameraOrtho.position.set(this.modelCenter.x, this.modelCenter.y + dist, this.modelCenter.z);
      this.cameraOrtho.lookAt(this.modelCenter);
      this.cameraOrtho.updateProjectionMatrix();
    } else {
      this.camera = this.cameraPersp;
      this.cameraPersp.up.set(0, 1, 0);
      switch (viewName) {
        case 'side':
          this.cameraPersp.position.set(this.modelCenter.x - dist, this.modelCenter.y, this.modelCenter.z);
          break;
        case 'bow':
          this.cameraPersp.position.set(this.modelCenter.x, this.modelCenter.y, this.modelCenter.z - dist);
          break;
        case 'stern':
          this.cameraPersp.position.set(this.modelCenter.x, this.modelCenter.y, this.modelCenter.z + dist);
          break;
        case 'iso':
        default:
          this.cameraPersp.position.set(
            this.modelCenter.x + dist * 0.75,
            this.modelCenter.y + dist * 0.55,
            this.modelCenter.z + dist * 0.75
          );
          break;
      }
    }
    this.controls.object = this.camera;
    this.controls.target.copy(this.modelCenter);
    this.controls.update();
  }

  captureSnapshot(format = 'image/png') {
    this.renderer.render(this.scene, this.camera);
    return this.renderer.domElement.toDataURL(format);
  }

  onResize() {
    if (!this.container) return;
    const width = this.container.clientWidth;
    const height = this.container.clientHeight;
    if (!width || !height) return;

    this.cameraPersp.aspect = width / height;
    this.cameraPersp.updateProjectionMatrix();
    const halfHeight = Math.max(this.modelRadius * 1.15, this.blockMeters * 4);
    this.cameraOrtho.left = -halfHeight * (width / height);
    this.cameraOrtho.right = halfHeight * (width / height);
    this.cameraOrtho.top = halfHeight;
    this.cameraOrtho.bottom = -halfHeight;
    this.cameraOrtho.updateProjectionMatrix();

    this.renderer.setSize(width, height);
  }

  animate(frameTime) {
    requestAnimationFrame(this.animate);
    const time = this.testMode ? 0 : frameTime;

    // Dynamic Pulsing Hologram Fresnel Glow
    if (this.hologramMaterial) {
      const pulse = (Math.sin(time * 0.0055) + 1) * 0.5;
      this.hologramMaterial.opacity = 0.5 + pulse * 0.35;
      this.hologramMaterial.emissiveIntensity = 0.5 + pulse * 0.5;
    }

    this.controls.update();
    this.renderer.render(this.scene, this.camera);
  }
}
