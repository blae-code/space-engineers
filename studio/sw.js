const CACHE_NAME = 'se3d-visualizer-v4';
const ASSETS = [
  './',
  './index.html',
  './manifest.json',
  './icon.svg',
  './css/app.css',
  './js/app.js',
  './js/VoxelWorld.js',
  './js/Slicer2D.js',
  './js/SnapshotApi.js',
  './vendor/three.module.js',
  './vendor/three.core.js',
  './vendor/OrbitControls.js',
  './js/OrbitControls.js',
  './data/chapters.json',
  './data/mastodon.json',
  './data/orca.json',
  './data/remora.json',
  './data/mule.json',
  './data/ground-base.json',
  './data/extruder-ring.json',
  './data/floating-rig.json',
  './data/caterpillar.json',
  './data/kestrel.json',
  './data/albatross.json'
];

self.addEventListener('install', (event) => {
  event.waitUntil(
    caches.open(CACHE_NAME).then((cache) => cache.addAll(ASSETS)).then(() => self.skipWaiting())
  );
});

self.addEventListener('activate', (event) => {
  event.waitUntil(
    caches.keys().then((keys) =>
      Promise.all(keys.filter((key) => key !== CACHE_NAME).map((key) => caches.delete(key)))
    ).then(() => self.clients.claim())
  );
});

self.addEventListener('fetch', (event) => {
  event.respondWith(
    caches.match(event.request).then((cachedResponse) => {
      if (cachedResponse) {
        return cachedResponse;
      }
      return fetch(event.request).catch(() => {
        if (event.request.mode === 'navigate') {
          return caches.match('./index.html');
        }
      });
    })
  );
});
