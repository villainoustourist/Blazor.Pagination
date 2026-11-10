using BlazorPagination;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BlazorPagination.Tests;

public class ExtensionsTests
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }

    private class ItemContext(DbContextOptions<ItemContext> options) : DbContext(options)
    {
        public DbSet<Item> Items => Set<Item>();
    }

    private static List<Item> MakeItems(int count) =>
        Enumerable.Range(1, count).Select(i => new Item { Id = i, Name = $"Item {i:D2}" }).ToList();

    private static ItemContext MakeContext(int count)
    {
        var options = new DbContextOptionsBuilder<ItemContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new ItemContext(options);
        context.Items.AddRange(MakeItems(count));
        context.SaveChanges();
        return context;
    }

    [Fact]
    public void ToPagedResult_ReturnsRequestedPage()
    {
        var result = MakeItems(23).AsQueryable().ToPagedResult(2, 10);

        Assert.Equal(2, result.CurrentPage);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(23, result.RowCount);
        Assert.Equal(3, result.PageCount);
        Assert.Equal(Enumerable.Range(11, 10), result.Results.Select(x => x.Id));
        Assert.Equal(11, result.FirstRowOnPage);
        Assert.Equal(20, result.LastRowOnPage);
    }

    [Fact]
    public void ToPagedResult_LastPageIsPartial()
    {
        var result = MakeItems(23).AsQueryable().ToPagedResult(3, 10);

        Assert.Equal(new[] { 21, 22, 23 }, result.Results.Select(x => x.Id));
        Assert.Equal(21, result.FirstRowOnPage);
        Assert.Equal(23, result.LastRowOnPage);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void ToPagedResult_PageBelowOneIsTreatedAsFirstPage(int page)
    {
        var result = MakeItems(23).AsQueryable().ToPagedResult(page, 10);

        Assert.Equal(1, result.CurrentPage);
        Assert.Equal(Enumerable.Range(1, 10), result.Results.Select(x => x.Id));
    }

    [Fact]
    public void ToPagedResult_PageAfterLastReturnsLastPageRows()
    {
        var result = MakeItems(23).AsQueryable().ToPagedResult(99, 10);

        Assert.Equal(3, result.PageCount);
        Assert.Equal(new[] { 21, 22, 23 }, result.Results.Select(x => x.Id));
    }

    [Fact]
    public void ToPagedResult_EmptySourceHasNoPages()
    {
        var result = MakeItems(0).AsQueryable().ToPagedResult(1, 10);

        Assert.Equal(0, result.RowCount);
        Assert.Equal(0, result.PageCount);
        Assert.Empty(result.Results);
        Assert.Equal(0, result.FirstRowOnPage);
        Assert.Equal(0, result.LastRowOnPage);
    }

    [Fact]
    public void ToPagedResult_ExactMultipleHasNoExtraPage()
    {
        var result = MakeItems(20).AsQueryable().ToPagedResult(1, 10);

        Assert.Equal(2, result.PageCount);
    }

    [Fact]
    public async Task ToPagedResultAsync_PagesAnEfCoreQuery()
    {
        using var context = MakeContext(23);

        var result = await context.Items.OrderBy(x => x.Id).ToPagedResultAsync(2, 10);

        Assert.Equal(2, result.CurrentPage);
        Assert.Equal(23, result.RowCount);
        Assert.Equal(3, result.PageCount);
        Assert.Equal(Enumerable.Range(11, 10), result.Results.Select(x => x.Id));
    }

    [Fact]
    public async Task ToPagedResultAsync_EmptyEfCoreQueryHasNoPages()
    {
        using var context = MakeContext(0);

        var result = await context.Items.ToPagedResultAsync(1, 10);

        Assert.Equal(0, result.RowCount);
        Assert.Equal(0, result.PageCount);
        Assert.Empty(result.Results);
    }

    [Fact]
    public void OrderBy_SortsAscendingByColumnName()
    {
        var items = MakeItems(5).OrderByDescending(x => x.Id).AsQueryable();

        var sorted = items.OrderBy("Id", "asc").Select(x => x.Id);

        Assert.Equal(new[] { 1, 2, 3, 4, 5 }, sorted);
    }

    [Fact]
    public void OrderBy_SortsDescendingWhenDirectionIsDesc()
    {
        var sorted = MakeItems(5).AsQueryable().OrderBy("Name", "desc").Select(x => x.Id);

        Assert.Equal(new[] { 5, 4, 3, 2, 1 }, sorted);
    }

    [Fact]
    public void OrderBy_UnknownColumnLeavesOrderUnchanged()
    {
        var items = new[] { 3, 1, 2 }.Select(i => new Item { Id = i }).AsQueryable();

        var sorted = items.OrderBy("DoesNotExist", "asc").Select(x => x.Id);

        Assert.Equal(new[] { 3, 1, 2 }, sorted);
    }

    [Fact]
    public void OrderBy_WorksOnEfCoreQuery()
    {
        using var context = MakeContext(5);

        var sorted = context.Items.OrderBy("Id", "desc").Select(x => x.Id).ToList();

        Assert.Equal(new[] { 5, 4, 3, 2, 1 }, sorted);
    }
}
