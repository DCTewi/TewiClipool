self.addEventListener('install', async () => {
    console.log('Installing service worker...');
    self.skipWaiting();
});

self.addEventListener('fetch', () => {
    return null;
});