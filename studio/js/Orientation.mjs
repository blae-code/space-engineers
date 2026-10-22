const DEGREES_TO_RADIANS = Math.PI / 180;

export function mapSpaceEngineersDirectionToThree(direction) {
  if (!Array.isArray(direction) || direction.length !== 3 ||
      !direction.every((value) => Number.isFinite(value))) {
    throw new TypeError('Direction must be a finite 3-element array');
  }
  return [direction[0], direction[1], direction[2] === 0 ? 0 : -direction[2]];
}

export function toThreeEulerRadians(rotationDegrees) {
  if (!Array.isArray(rotationDegrees) || rotationDegrees.length !== 3 ||
      !rotationDegrees.every((value) => Number.isFinite(value))) {
    throw new TypeError('Rotation must be a finite 3-element degree array');
  }
  const [x, y, z] = rotationDegrees;
  return [
    x === 0 ? 0 : -x * DEGREES_TO_RADIANS,
    y === 0 ? 0 : -y * DEGREES_TO_RADIANS,
    z * DEGREES_TO_RADIANS
  ];
}
