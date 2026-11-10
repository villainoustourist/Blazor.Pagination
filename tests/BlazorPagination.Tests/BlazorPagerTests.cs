using Bunit;
using Xunit;

namespace BlazorPagination.Tests;

public class BlazorPagerTests : BunitContext
{
    private IRenderedComponent<BlazorPager> RenderPager(int pageCount, int currentPage, int visiblePages = 5,
        bool showFirstLast = false, bool showPageNumbers = true, Func<int, Task>? onPageChanged = null) =>
        Render<BlazorPager>(ps => ps
            .Add(p => p.PageCount, pageCount)
            .Add(p => p.CurrentPage, currentPage)
            .Add(p => p.VisiblePages, visiblePages)
            .Add(p => p.ShowFirstLast, showFirstLast)
            .Add(p => p.ShowPageNumbers, showPageNumbers)
            .Add(p => p.OnPageChanged, onPageChanged));

    // The visible label of each pager item, ignoring the screen-reader text.
    private static List<string> Labels(IRenderedComponent<BlazorPager> cut) =>
        cut.FindAll("li.page-item a.page-link")
            .Select(a => (a.QuerySelector("span[aria-hidden]") ?? a).TextContent.Trim())
            .ToList();

    private static List<string> PageNumbers(IRenderedComponent<BlazorPager> cut) =>
        Labels(cut).Where(l => int.TryParse(l, out _)).ToList();

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void RendersNothingWithOnePageOrLess(int pageCount)
    {
        var cut = RenderPager(pageCount, 1);

        Assert.Empty(cut.FindAll("nav"));
    }

    [Fact]
    public void RendersPreviousPagesAndNext()
    {
        var cut = RenderPager(10, 1);

        Assert.Equal(new[] { "Previous", "1", "2", "3", "4", "5", "Next" }, Labels(cut));
    }

    [Fact]
    public void MarksCurrentPageActive()
    {
        var cut = RenderPager(10, 4);

        var active = Assert.Single(cut.FindAll("li.page-item.active"));
        Assert.Equal("4", active.TextContent.Trim());
    }

    [Fact]
    public void CentersWindowOnCurrentPage()
    {
        var cut = RenderPager(10, 5);

        Assert.Equal(new[] { "3", "4", "5", "6", "7" }, PageNumbers(cut));
    }

    [Fact]
    public void ShowsAllPagesWhenFewerThanVisiblePages()
    {
        var cut = RenderPager(3, 2);

        Assert.Equal(new[] { "1", "2", "3" }, PageNumbers(cut));
    }

    [Fact]
    public void DisablesPreviousOnFirstPage()
    {
        var cut = RenderPager(10, 1);

        Assert.Contains("disabled", cut.FindAll("li.page-item")[0].ClassList);
        Assert.DoesNotContain("disabled", cut.FindAll("li.page-item")[^1].ClassList);
    }

    [Fact]
    public void DisablesNextOnLastPage()
    {
        var cut = RenderPager(10, 10);

        Assert.DoesNotContain("disabled", cut.FindAll("li.page-item")[0].ClassList);
        Assert.Contains("disabled", cut.FindAll("li.page-item")[^1].ClassList);
    }

    [Fact]
    public void ShowsFirstAndLastWhenEnabled()
    {
        var cut = RenderPager(10, 5, showFirstLast: true);

        var labels = Labels(cut);
        Assert.Equal("First", labels[0]);
        Assert.Equal("Last", labels[^1]);
    }

    [Fact]
    public void HidesFirstAndLastByDefault()
    {
        var cut = RenderPager(10, 5);

        Assert.DoesNotContain("First", Labels(cut));
        Assert.DoesNotContain("Last", Labels(cut));
    }

    [Fact]
    public void HidesPageNumbersWhenDisabled()
    {
        var cut = RenderPager(10, 5, showPageNumbers: false);

        Assert.Equal(new[] { "Previous", "Next" }, Labels(cut));
    }

    [Fact]
    public void UsesCustomTexts()
    {
        var cut = Render<BlazorPager>(ps => ps
            .Add(p => p.PageCount, 10)
            .Add(p => p.CurrentPage, 5)
            .Add(p => p.ShowFirstLast, true)
            .Add(p => p.FirstText, "«")
            .Add(p => p.PreviousText, "‹")
            .Add(p => p.NextText, "›")
            .Add(p => p.LastText, "»"));

        var labels = Labels(cut);
        Assert.Equal(new[] { "«", "‹" }, labels.Take(2));
        Assert.Equal(new[] { "›", "»" }, labels.TakeLast(2));
    }

    [Theory]
    [InlineData("4", 4)]
    [InlineData("Previous", 4)]
    [InlineData("Next", 6)]
    [InlineData("First", 1)]
    [InlineData("Last", 10)]
    public void ClickingAnItemRaisesOnPageChanged(string label, int expectedPage)
    {
        int? changedTo = null;
        var cut = RenderPager(10, 5, showFirstLast: true, onPageChanged: p => { changedTo = p; return Task.CompletedTask; });

        var link = cut.FindAll("li.page-item a.page-link")
            .Single(a => (a.QuerySelector("span[aria-hidden]") ?? a).TextContent.Trim() == label);
        link.Click();

        Assert.Equal(expectedPage, changedTo);
    }

    [Fact]
    public void ClickingWithoutHandlerDoesNotThrow()
    {
        var cut = RenderPager(10, 5);

        cut.FindAll("li.page-item a.page-link").Single(a => a.TextContent.Trim() == "6").Click();
    }
}
