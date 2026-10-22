const CACHE_NAME = 'se3d-visualizer-v10';
const SHELL_ASSETS = [
  './',
  './index.html',
  './manifest.json',
  './icon.svg',
  './icon-180.png',
  './icon-192.png',
  './icon-512.png',
  './css/app.css',
  './js/app.js',
  './js/VoxelWorld.js',
  './js/Slicer2D.js',
  './js/SnapshotApi.js',
  './vendor/three.module.js',
  './vendor/three.core.js',
  './vendor/OrbitControls.js',
  './js/OrbitControls.js',
  './data/chapters.json'
];

self.addEventListener('install', (event) => {
  event.waitUntil((async () => {
    const registryResponse = await fetch('./data/chapters.json');
    if (!registryResponse.ok) {
      throw new Error(`Chapter registry precache failed (${registryResponse.status})`);
    }
    const chapters = await registryResponse.json();
    if (!Array.isArray(chapters) || chapters.some((chapter) =>
      !chapter || typeof chapter.file !== 'string' || !/^[a-zA-Z0-9_-]+\.json$/.test(chapter.file)
    )) {
      throw new Error('Chapter registry contains invalid model paths');
    }
    const chapterAssets = chapters.map((chapter) => `./data/${encodeURIComponent(chapter.file)}`);
    const cache = await caches.open(CACHE_NAME);
    await cache.addAll([...SHELL_ASSETS, ...chapterAssets]);
  })());
});

self.addEventListener('message', (event) => {
  if (event.data && event.data.type === 'SKIP_WAITING') {
    event.waitUntil(self.skipWaiting());
  }
});

self.addEventListener('activate', (event) => {
  event.waitUntil(
    caches.keys().then((keys) =>
      Promise.all(keys.filter((key) => key !== CACHE_NAME && key.startsWith('se3d-')).map((key) => caches.delete(key)))
    ).then(() => self.clients.claim())
  );
});

self.addEventListener('fetch', (event) => {
  if (event.request.method !== 'GET') return;
  const registryUrl = new URL('./data/chapters.json', self.location.href);
  if (new URL(event.request.url).pathname === registryUrl.pathname) {
    event.respondWith(fetchRegistryAndRefreshModels(event.request));
    return;
  }
  event.respondWith((async () => {
    const cached = await caches.match(event.request);
    if (cached) return cached;
    try {
      return await fetch(event.request);
    } catch {
      if (event.request.mode === 'navigate') {
        const fallback = await caches.match('./index.html');
        if (fallback) return fallback;
      }
      return new Response('This resource is unavailable while offline.', {
        status: 503,
        statusText: 'Offline',
        headers: { 'Content-Type': 'text/plain; charset=utf-8' }
      });
    }
  })());
});

async function fetchRegistryAndRefreshModels(request) {
  try {
    const response = await fetch(request);
    if (response.ok) {
      try {
        const chapters = await response.clone().json();
        if (!Array.isArray(chapters) || chapters.some((chapter) =>
          !chapter || typeof chapter.file !== 'string' || !/^[a-zA-Z0-9_-]+\.json$/.test(chapter.file)
        )) {
          throw new Error('Chapter registry contains invalid model paths');
        }
        const cache = await caches.open(CACHE_NAME);
        const models = await Promise.all(chapters.map(async (chapter) => {
          const modelUrl = new URL(`./data/${encodeURIComponent(chapter.file)}`, self.location.href);
          const modelResponse = await fetch(modelUrl);
          if (!modelResponse.ok) {
            throw new Error(`Precache failed for ${chapter.file} (${modelResponse.status})`);
          }
          return [modelUrl, modelResponse];
        }));
        await Promise.all(models.map(([url, modelResponse]) => cache.put(url, modelResponse)));
        await cache.put(request, response.clone());
      } catch (error) {
        console.error('Unable to refresh the offline chapter cache:', error);
      }
    }
    return response;
  } catch (error) {
    const cached = await caches.match(request);
    if (cached) return cached;
    console.error('Unable to fetch the chapter registry:', error);
    return new Response('The chapter registry is unavailable while offline.', {
      status: 503,
      statusText: 'Offline',
      headers: { 'Content-Type': 'text/plain; charset=utf-8' }
    });
  }
}
