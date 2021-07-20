using System;
using System.Threading.Tasks;
using PlaywrightSharp;
using PlaywrightSharp.Chromium;

namespace RegPlaywright.Controller
{
    class ChroniumReg : IDisposable
    {
        public IPlaywright Playwright { get; set; }
        public IChromiumBrowserContext Browser { get; set; }
        public IPage Page { get; set; }

        public bool IsNote { get; set; }
        public int Index { get; set; }
        public void Dispose()
        {
            Dispose(true);
            GC.Collect();
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Page?.CloseAsync();
                Browser?.CloseAsync();
                Browser = Playwright?.Chromium.LaunchAsync().Result.NewContextAsync().Result;
                Browser?.CloseAsync();
                Browser?.DisposeAsync();
                Playwright?.Dispose();
            }
        }
        public void DisposeBrowser()
        {
            DisposeBrowser(true);
            GC.Collect();
            GC.SuppressFinalize(this);
        }
        private void DisposeBrowser(bool disposing)
        {
            if (disposing)
            {
                Browser?.CloseAsync();
                Browser = Playwright?.Chromium.LaunchAsync().Result.NewContextAsync().Result;
                Browser?.CloseAsync();
                Browser?.DisposeAsync();
            }
        }
    }
}
