// See https://aka.ms/new-console-template for more information
//using Microsoft.Playwright;


using Microsoft.Playwright;
using Xunit;

namespace Microsoft.AspNetCore.SystemWebAdapters.Playwrights
{

    public class PlaywrightFixture : IAsyncDisposable, IDisposable
    {
        private readonly Task<IPlaywright> _playwright;
        private readonly Task<IBrowser> _browser;

        public PlaywrightFixture()
        {
            _playwright = Playwright.Playwright.CreateAsync();
            _browser = LaunchAsync();

            async Task<IBrowser> LaunchAsync()
            {
                var playwright = await _playwright;
                return await playwright.Chromium.LaunchAsync();
            }
        }

        public string CoreApp => "http://localhost:9000";

        public string MvcApp => "http://localhost:9010";

        public Task<IBrowser> GetBrowserAsync() => _browser;

        public void Dispose() => DisposeAsync().GetAwaiter().GetResult();

        public async ValueTask DisposeAsync()
        {
            using var _ = await _playwright;
            await using var __ = await _browser;
        }
    }

    [CollectionDefinition(nameof(PlaywrightCollection))]
    public class PlaywrightCollection : ICollectionFixture<PlaywrightFixture>
    {

    }

    [Collection(nameof(PlaywrightCollection))]
    public class RequestTests
    {
        private readonly PlaywrightFixture _playwright;

        public RequestTests(PlaywrightFixture playwright)
        {
            _playwright = playwright;
        }

        [Fact]
        public async Task Run()
        {
            var browser = await _playwright.GetBrowserAsync();

            await using var context = await browser.NewContextAsync(new BrowserNewContextOptions() { BaseURL = _playwright.MvcApp });
            var page = await context.NewPageAsync();

            var response = await page.GotoAsync("/api/test/request/info");

            var headers = await response.AllHeadersAsync();
            var cookies = await context.CookiesAsync();
            var content = await response.BodyAsync();

            Console.WriteLine("blah");
        }
    }
}
