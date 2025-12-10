using Microsoft.Playwright;
using NUnit.Framework;

namespace X00194620_Ca3.Tests.E2E;

[Parallelizable(ParallelScope.Self)]
public class E2E_LaunchNavigationTests{
    private IPlaywright _playwright = null!;
    private IBrowser _browser = null!;
    private const string BaseUrl = "https://adriandonnelly.github.io/CA3_SpaceXLaunches/";

    [SetUp]
    public async Task Setup(){
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions{
            Headless = true
        });
    }

    [TearDown]
    public async Task TearDown(){
        await _browser.CloseAsync();
        _playwright.Dispose();
    }

    [Test]
    public async Task TestHomePageTitle(){
        var page = await _browser.NewPageAsync();
        await page.GotoAsync(BaseUrl);
        await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        await page.WaitForSelectorAsync("text=SpaceX Launch Timeline", new(){ Timeout = 60000 });
        Assert.IsTrue(await page.IsVisibleAsync("text=SpaceX Launch Timeline"));
    }

    [Test]
    public async Task TestLaunchCards(){
        var page = await _browser.NewPageAsync();
        await page.GotoAsync(BaseUrl);
        await page.WaitForSelectorAsync("[data-test-id='launch-card']", new(){ Timeout = 60000 });
        var cards = await page.QuerySelectorAllAsync("[data-test-id='launch-card']");
        Assert.That(cards.Count, Is.GreaterThan(0));
    }

    [Test]
    public async Task TestNavLinkToDetails(){
        var page = await _browser.NewPageAsync();
        await page.GotoAsync(BaseUrl);
        await page.WaitForSelectorAsync("[data-test-id='launch-card']", new(){ Timeout = 60000 });  
        var drawerOverlay = await page.QuerySelectorAsync(".mud-overlay-scrim");
        if (drawerOverlay != null && await drawerOverlay.IsVisibleAsync()){
            await page.ClickAsync(".mud-overlay-scrim");
            await Task.Delay(500);
        }
        await page.ClickAsync("[data-test-id='launch-card'] >> nth=0");
        await page.WaitForURLAsync(url => url.ToString().Contains("/launch/"),
            new(){ Timeout = 60000 });
        StringAssert.Contains("/launch/", page.Url);
    }

    [Test]
    public async Task TestNavLinkToPhotos(){
        var page = await _browser.NewPageAsync();
        await page.GotoAsync(BaseUrl);
        await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        var drawerButton = await page.QuerySelectorAsync("button[aria-label='Open drawer']");
        if (drawerButton != null){
            await page.ClickAsync("button[aria-label='Open drawer']");
            await Task.Delay(500);
        }
        await page.ClickAsync("text=Launch Photos");
        await page.WaitForURLAsync(url => url.ToString().Contains("/photos"),
            new(){ Timeout = 60000 });
        StringAssert.Contains("/photos", page.Url);
    }

    [Test]
    public async Task TestNavToStatistics(){
        var page = await _browser.NewPageAsync();
        await page.GotoAsync(BaseUrl);
        await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        var drawerButton = await page.QuerySelectorAsync("button[aria-label='Open drawer']");
        if (drawerButton != null){
            await page.ClickAsync("button[aria-label='Open drawer']");
            await Task.Delay(500);
        }
        await page.ClickAsync("text=Statistics");
        await page.WaitForURLAsync(url => url.ToString().Contains("/statistics"),
            new(){ Timeout = 60000 });
        StringAssert.Contains("/statistics", page.Url);
    }

    [Test]
    public async Task TestDarkMode(){
        var page = await _browser.NewPageAsync();
        await page.GotoAsync(BaseUrl);
        await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        await page.WaitForSelectorAsync("text=SpaceX Launches", new(){ Timeout = 60000 });
        var drawerOverlay = await page.QuerySelectorAsync(".mud-overlay-scrim");
        if (drawerOverlay != null && await drawerOverlay.IsVisibleAsync()){
            await page.ClickAsync(".mud-overlay-scrim");
            await Task.Delay(500);
        }
        var darkModeButtons = await page.QuerySelectorAllAsync(".mud-appbar button.mud-icon-button");
        Assert.That(darkModeButtons.Count, Is.GreaterThan(0), "Should have icon buttons in app bar");
    }
}