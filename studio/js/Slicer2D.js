export class Slicer2D {
  static getSymbolForShape(shape, subtype = '') {
    switch (shape) {
      case 'cube':
        return '■';
      case 'half':
      case 'halfslope':
        return '▲';
      case 'slope2x1':
        return '◢';
      case 'conveyor':
        return '○';
      case 'wheel5x5':
        return 'W';
      case 'reactor':
        return 'R';
      case 'battery':
        return 'B';
      case 'cargo':
      case 'cargo_small':
        return 'C';
      case 'cockpit':
        return 'K';
      case 'turret':
        return 'T';
      case 'rotor':
      case 'hinge':
        return 'H';
      case 'piston':
        return 'P';
      case 'refinery':
        return 'F';
      case 'assembler':
        return 'A';
      case 'generator':
      case 'engine':
        return 'E';
      case 'solar':
        return 'S';
      case 'catwalk':
        return '░';
      case 'welder':
        return '+';
      case 'tank':
        return 'H2';
      default:
        return '■';
    }
  }

  static generateAsciiSlice(modelData, targetLevel, maxStepIndex = 9999) {
    const levelNum = parseInt(targetLevel.replace('L', ''), 10) || 1;
    const minX = modelData.bounds.min[0];
    const maxX = modelData.bounds.max[0];
    const minZ = modelData.bounds.min[2];
    const maxZ = modelData.bounds.max[2];

    const grid = {};
    const terminalList = [];

    modelData.steps.forEach((step, sIdx) => {
      if (sIdx > maxStepIndex) return;

      step.blocks.forEach((b) => {
        if (b.pos[1] === levelNum) {
          const key = `${b.pos[0]},${b.pos[2]}`;
          grid[key] = {
            symbol: Slicer2D.getSymbolForShape(b.shape, b.subtype),
            name: b.name || b.subtype || b.shape,
            subsystem: b.subsystem,
            slot: b.paletteSlot,
            pos: b.pos
          };
          if (b.name) {
            terminalList.push(`[X:${b.pos[0] >= 0 ? '+' : ''}${b.pos[0]}, Z:${b.pos[2] >= 0 ? '+' : ''}${b.pos[2]}] · ${b.name} (${b.subsystem})`);
          }
        }
      });
    });

    let lines = [];
    lines.push(`========================================================================================`);
    lines.push(`SPACE ENGINEERS 2D DECK SLICE · ${modelData.name} (${modelData.id})`);
    lines.push(`Level: ${targetLevel} (Y = ${levelNum}) · Step Limit: <= ${maxStepIndex}`);
    lines.push(`Orientation: ▲ Forward / Bow (+Z) · ◄ Port (-X) · Starboard (+X) ►`);
    lines.push(`========================================================================================`);

    // X axis header
    let header = ' Z \\ X   ';
    for (let x = minX; x <= maxX; x++) {
      header += (x >= 0 ? ` +${x}` : ` ${x}`).padEnd(4, ' ');
    }
    lines.push(header);
    lines.push('--------+' + '----'.repeat(maxX - minX + 1));

    // Rows from Bow (+Z) to Stern (-Z)
    for (let z = maxZ; z >= minZ; z--) {
      let rowStr = (z >= 0 ? ` +${z}` : ` ${z}`).padStart(5, ' ') + '  │ ';
      let hasAnyBlockInRow = false;

      for (let x = minX; x <= maxX; x++) {
        const item = grid[`${x},${z}`];
        if (item) {
          rowStr += ` ${item.symbol}  `;
          hasAnyBlockInRow = true;
        } else {
          rowStr += ' ·  ';
        }
      }

      if (hasAnyBlockInRow || z % 5 === 0) {
        lines.push(rowStr);
      }
    }

    lines.push(`Legend: ■ Armor Block   ○ Conveyor   ▲ Half/Bevel   ◢ 2x1 Slope   W 5x5 Wheel`);
    lines.push(`        B Battery       R Reactor    C Cargo Hold   K Cockpit     T Turret      H Rotor/Hinge`);
    lines.push(`        F Refinery      A Assembler  E H2 Engine    S Solar       P Piston      ░ Catwalk`);
    lines.push(`        + Welder        H2 Tank`);

    if (terminalList.length > 0) {
      lines.push('');
      lines.push(`Terminal Blocks & Critical Devices on Level ${targetLevel} (${terminalList.length} items):`);
      terminalList.forEach((t) => lines.push(`  · ${t}`));
    }

    return lines.join('\n');
  }

  static generateSvgSlice(modelData, targetLevel, maxStepIndex = 9999, showFoundationGhost = true) {
    const levelNum = parseInt(targetLevel.replace('L', ''), 10) || 1;
    const minX = modelData.bounds.min[0];
    const maxX = modelData.bounds.max[0];
    const minZ = modelData.bounds.min[2];
    const maxZ = modelData.bounds.max[2];

    const cellSize = 32;
    const padding = 50;
    const cols = maxX - minX + 1;
    const rows = maxZ - minZ + 1;
    const width = cols * cellSize + padding * 2;
    const height = rows * cellSize + padding * 2;

    let svg = `<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 ${width} ${height}" width="100%" height="auto" class="deck-slice-svg">`;
    svg += `<rect width="${width}" height="${height}" fill="#0a0e14"/>`;

    // 2.5 m Grid lines
    svg += `<g stroke="#1a2330" stroke-width="1">`;
    for (let c = 0; c <= cols; c++) {
      const x = padding + c * cellSize;
      svg += `<line x1="${x}" y1="${padding}" x2="${x}" y2="${height - padding}"/>`;
    }
    for (let r = 0; r <= rows; r++) {
      const y = padding + r * cellSize;
      svg += `<line x1="${padding}" y1="${y}" x2="${width - padding}" y2="${y}"/>`;
    }
    svg += `</g>`;

    // Centerline Datum Highlights (X = 0, Z = 0)
    const midX = padding + (0 - minX) * cellSize;
    const midZ = padding + (maxZ - 0) * cellSize;
    svg += `<line x1="${midX}" y1="${padding}" x2="${midX}" y2="${height - padding}" stroke="#e6a10044" stroke-width="2"/>`;
    svg += `<line x1="${padding}" y1="${midZ}" x2="${width - padding}" y2="${midZ}" stroke="#e6a10022" stroke-width="1.5"/>`;

    // Layer Below (Foundation Ghost) if level > 1
    if (showFoundationGhost && levelNum > 1) {
      const lowerLevel = levelNum - 1;
      modelData.steps.forEach((step, sIdx) => {
        if (sIdx > maxStepIndex) return;
        step.blocks.forEach((b) => {
          if (b.pos[1] === lowerLevel) {
            const px = padding + (b.pos[0] - minX) * cellSize;
            const py = padding + (maxZ - b.pos[2]) * cellSize;
            svg += `<rect x="${px + 2}" y="${py + 2}" width="${cellSize - 4}" height="${cellSize - 4}" fill="#16202c" stroke="#223040" stroke-dasharray="2,2" stroke-width="1" opacity="0.6"/>`;
          }
        });
      });
    }

    // Active Level Blocks
    modelData.steps.forEach((step, sIdx) => {
      if (sIdx > maxStepIndex) return;

      step.blocks.forEach((b) => {
        if (b.pos[1] === levelNum) {
          const px = padding + (b.pos[0] - minX) * cellSize;
          const py = padding + (maxZ - b.pos[2]) * cellSize;
          const slot = modelData.palette[b.paletteSlot || 0];
          const color = slot ? slot.hex : '#42464D';

          const isActiveStep = (sIdx === maxStepIndex);
          const strokeColor = isActiveStep ? '#1abc9c' : '#000000';
          const strokeWidth = isActiveStep ? 2 : 1;

          svg += `<g class="slice-block-node" data-pos="${b.pos.join(',')}" data-name="${b.name || b.subtype || b.shape}" data-sub="${b.subsystem}">`;
          svg += `<rect x="${px + 2}" y="${py + 2}" width="${cellSize - 4}" height="${cellSize - 4}" rx="3" fill="${color}" stroke="${strokeColor}" stroke-width="${strokeWidth}"/>`;

          const symbol = Slicer2D.getSymbolForShape(b.shape);
          svg += `<text x="${px + cellSize / 2}" y="${py + cellSize / 2 + 4}" fill="#f2f5f6" font-family="'Space Mono', monospace" font-size="11" font-weight="bold" text-anchor="middle" pointer-events="none">${symbol}</text>`;
          svg += `<title>[X:${b.pos[0]}, Z:${b.pos[2]}] ${b.name || b.subtype || b.shape} (${b.subsystem})</title>`;
          svg += `</g>`;
        }
      });
    });

    // Cardinal Header Banners
    svg += `<text x="${width / 2}" y="28" fill="#e6a100" font-family="'Barlow Condensed', sans-serif" font-size="15" font-weight="bold" letter-spacing="0.1em" text-anchor="middle">▲ FORWARD / BOW (+Z)</text>`;
    svg += `<text x="${width / 2}" y="${height - 18}" fill="#aab8c8" font-family="'Barlow Condensed', sans-serif" font-size="13" font-weight="bold" letter-spacing="0.1em" text-anchor="middle">▼ AFT / STERN (-Z)</text>`;
    svg += `<text x="20" y="${height / 2}" fill="#aab8c8" font-family="'Barlow Condensed', sans-serif" font-size="13" font-weight="bold" letter-spacing="0.1em" text-anchor="middle" transform="rotate(-90 20, ${height / 2})">◄ PORT (-X)</text>`;
    svg += `<text x="${width - 20}" y="${height / 2}" fill="#aab8c8" font-family="'Barlow Condensed', sans-serif" font-size="13" font-weight="bold" letter-spacing="0.1em" text-anchor="middle" transform="rotate(90 ${width - 20}, ${height / 2})">STARBOARD (+X) ►</text>`;

    svg += `</svg>`;
    return svg;
  }
}
