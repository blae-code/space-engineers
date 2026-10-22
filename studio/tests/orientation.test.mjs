import assert from 'node:assert/strict';
import test from 'node:test';
import {
  mapSpaceEngineersDirectionToThree,
  toThreeEulerRadians
} from '../js/Orientation.mjs';

function rotateXYZ(vector, angles) {
  const [x, y, z] = angles;
  const [vx, vy, vz] = vector;
  const afterX = [vx, vy * Math.cos(x) - vz * Math.sin(x), vy * Math.sin(x) + vz * Math.cos(x)];
  const afterY = [
    afterX[0] * Math.cos(y) + afterX[2] * Math.sin(y),
    afterX[1],
    -afterX[0] * Math.sin(y) + afterX[2] * Math.cos(y)
  ];
  return [
    afterY[0] * Math.cos(z) - afterY[1] * Math.sin(z),
    afterY[0] * Math.sin(z) + afterY[1] * Math.cos(z),
    afterY[2]
  ];
}

function closeVector(actual, expected) {
  actual.forEach((value, index) => assert.ok(Math.abs(value - expected[index]) < 1e-10));
}

test('maps Space Engineers cardinal directions into the reflected Three.js frame', () => {
  assert.deepEqual(mapSpaceEngineersDirectionToThree([1, 0, 0]), [1, 0, 0]);
  assert.deepEqual(mapSpaceEngineersDirectionToThree([0, 1, 0]), [0, 1, 0]);
  assert.deepEqual(mapSpaceEngineersDirectionToThree([0, 0, 1]), [0, 0, -1]);
  assert.deepEqual(mapSpaceEngineersDirectionToThree([0, 0, -1]), [0, 0, 1]);
});

test('converts degree rotations and preserves the reflected XYZ rotation', () => {
  const rotationDegrees = [31, -47, 122];
  const viewerAngles = toThreeEulerRadians(rotationDegrees);
  assert.deepEqual(toThreeEulerRadians([0, 0, 0]), [0, 0, 0]);
  closeVector(viewerAngles, [-31 * Math.PI / 180, 47 * Math.PI / 180, 122 * Math.PI / 180]);

  const direction = [0.3, -0.4, 0.5];
  const seRotated = rotateXYZ(direction, rotationDegrees.map((angle) => angle * Math.PI / 180));
  const expectedViewer = mapSpaceEngineersDirectionToThree(seRotated);
  const viewerInput = mapSpaceEngineersDirectionToThree(direction);
  closeVector(rotateXYZ(viewerInput, viewerAngles), expectedViewer);
});

test('rejects malformed rotations and directions', () => {
  assert.throws(() => toThreeEulerRadians([90, 0]), TypeError);
  assert.throws(() => toThreeEulerRadians([0, NaN, 0]), TypeError);
  assert.throws(() => mapSpaceEngineersDirectionToThree([0, 0, Infinity]), TypeError);
});
