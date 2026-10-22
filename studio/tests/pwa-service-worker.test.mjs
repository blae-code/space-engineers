import assert from 'node:assert/strict';
import { readFile } from 'node:fs/promises';
import test from 'node:test';
import vm from 'node:vm';

const serviceWorkerSource = await readFile(new URL('../sw.js', import.meta.url), 'utf8');
const chapterRegistry = JSON.parse(
  await readFile(new URL('../data/chapters.json', import.meta.url), 'utf8')
);

function createWorkerHarness() {
  const listeners = new Map();
  const calls = {
    cacheAssets: [],
    deletedCaches: [],
    skipWaiting: 0,
    clientsClaimed: 0
  };
  const cache = {
    addAll: async (assets) => {
      calls.cacheAssets = assets;
    }
  };
  const self = {
    location: { href: 'https://example.test/studio/sw.js' },
    clients: {
      claim: async () => {
        calls.clientsClaimed++;
      }
    },
    skipWaiting: async () => {
      calls.skipWaiting++;
    },
    addEventListener: (type, listener) => {
      listeners.set(type, listener);
    }
  };
  const caches = {
    open: async () => cache,
    keys: async () => ['se3d-visualizer-v9', 'unrelated-cache'],
    delete: async (key) => {
      calls.deletedCaches.push(key);
      return true;
    },
    match: async (request) => request === './index.html'
      ? new Response('cached app shell')
      : undefined
  };

  vm.runInNewContext(serviceWorkerSource, {
    self,
    caches,
    fetch: async (request) => {
      if (String(request).includes('chapters.json')) {
        return {
          ok: true,
          json: async () => chapterRegistry
        };
      }
      throw new Error('network unavailable');
    },
    URL,
    Response,
    encodeURIComponent,
    Promise,
    console
  });

  return { calls, listeners };
}

test('new worker waits for an explicit update request', async () => {
  const { calls, listeners } = createWorkerHarness();
  const installPromises = [];
  listeners.get('install')({ waitUntil: (promise) => installPromises.push(promise) });
  await Promise.all(installPromises);

  assert.equal(calls.skipWaiting, 0);
  assert.ok(calls.cacheAssets.includes('./icon-192.png'));
  assert.ok(calls.cacheAssets.includes('./icon-512.png'));
  for (const chapter of chapterRegistry) {
    assert.ok(calls.cacheAssets.includes(`./data/${encodeURIComponent(chapter.file)}`));
  }

  const messagePromises = [];
  listeners.get('message')({
    data: { type: 'SKIP_WAITING' },
    waitUntil: (promise) => messagePromises.push(promise)
  });
  await Promise.all(messagePromises);
  assert.equal(calls.skipWaiting, 1);
});

test('activation removes old app caches and claims current clients', async () => {
  const { calls, listeners } = createWorkerHarness();
  const activationPromises = [];
  listeners.get('activate')({ waitUntil: (promise) => activationPromises.push(promise) });
  await Promise.all(activationPromises);

  assert.deepEqual(calls.deletedCaches, ['se3d-visualizer-v9']);
  assert.equal(calls.clientsClaimed, 1);
});

test('offline navigation falls back to the cached app shell', async () => {
  const { listeners } = createWorkerHarness();
  const responsePromises = [];
  listeners.get('fetch')({
    request: {
      method: 'GET',
      mode: 'navigate',
      url: 'https://example.test/studio/unavailable'
    },
    respondWith: (promise) => responsePromises.push(promise)
  });
  const response = await responsePromises[0];

  assert.equal(response.status, 200);
  assert.equal(await response.text(), 'cached app shell');
});
