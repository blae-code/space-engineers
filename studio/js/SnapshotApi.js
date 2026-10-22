import { Slicer2D } from './Slicer2D.js';

export class SnapshotApi {
  static downloadFile(filename, content, type = 'text/plain') {
    const blob = new Blob([content], { type });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    window.setTimeout(() => URL.revokeObjectURL(url), 1000);
  }

  static downloadPng(dataUrl, filename = 'snapshot.png') {
    const a = document.createElement('a');
    a.href = dataUrl;
    a.download = filename;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
  }

  static escapeXml(value) {
    return String(value ?? '').replace(/[&<>"']/g, (character) => ({
      '&': '&amp;',
      '<': '&lt;',
      '>': '&gt;',
      '"': '&quot;',
      "'": '&apos;'
    })[character]);
  }

  static generateSbcXml(modelData, maxStepIndex = 9999) {
    let sbc = `<?xml version="1.0" encoding="utf-8"?>\n`;
    sbc += `<!-- Draft export of authored visual placements; verify against an in-game blueprint before use. -->\n`;
    sbc += `<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">\n`;
    sbc += `  <ShipBlueprints>\n`;
    sbc += `    <ShipBlueprint xsi:type="MyObjectBuilder_ShipBlueprintDefinition">\n`;
    sbc += `      <Id Type="MyObjectBuilder_ShipBlueprintDefinition" Subtype="${SnapshotApi.escapeXml(modelData.id)}" />\n`;
    sbc += `      <DisplayName>${SnapshotApi.escapeXml(modelData.name)}</DisplayName>\n`;
    sbc += `      <CubeGrids>\n`;
    sbc += `        <CubeGrid>\n`;
    sbc += `          <GridSizeEnum>${modelData.gridSize}</GridSizeEnum>\n`;
    sbc += `          <CubeBlocks>\n`;

    modelData.steps.forEach((step, sIdx) => {
      if (sIdx > maxStepIndex) return;

      step.blocks.forEach((b) => {
        const subtype = SnapshotApi.escapeXml(b.subtype || 'LargeHeavyBlockArmorBlock');
        sbc += `            <MyObjectBuilder_CubeBlock xsi:type="MyObjectBuilder_CubeBlock">\n`;
        sbc += `              <SubtypeName>${subtype}</SubtypeName>\n`;
        sbc += `              <Min x="${b.pos[0]}" y="${b.pos[1]}" z="${b.pos[2]}" />\n`;
        if (b.name) {
          sbc += `              <CustomName>${SnapshotApi.escapeXml(b.name)}</CustomName>\n`;
        }
        sbc += `            </MyObjectBuilder_CubeBlock>\n`;
      });
    });

    sbc += `          </CubeBlocks>\n`;
    sbc += `        </CubeGrid>\n`;
    sbc += `      </CubeGrids>\n`;
    sbc += `    </ShipBlueprint>\n`;
    sbc += `  </ShipBlueprints>\n`;
    sbc += `</Definitions>\n`;
    return sbc;
  }
}
