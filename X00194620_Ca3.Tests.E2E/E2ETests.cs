using Microsoft.Playwright;
using NUnit.Framework;

namespace X00194620_Ca3.Tests.E2E;

[Parallelizable(ParallelScope.Self)]
public class E2E_LaunchNavigationTests
{
    private IPlaywright _playwright = null!;
    private IBrowser _browser = null!;
    private const string BaseUrl = "https://adriandonnelly.github.io/CA3_SpaceXLaunches/";

    [SetUp]
    public async Task Setup()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
    }

    [TearDown]
    public async Task TearDown()
    {
        await _browser.CloseAsync();
        _playwright.Dispose();
    }

    [Test]
    public async Task HomePage_Should_Load_And_Display_Title()
    {
        var page = await _browser.NewPageAsync();
        await page.GotoAsync(BaseUrl);
        await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        await page.WaitForSelectorAsync("text=SpaceX Launch Timeline", new() { Timeout = 60000 });

        Assert.IsTrue(await page.IsVisibleAsync("text=SpaceX Launch Timeline"));
    }
}
