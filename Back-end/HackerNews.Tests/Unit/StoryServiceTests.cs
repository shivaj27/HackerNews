using FluentAssertions;
using HackerNews.Models;
using HackerNews.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace HackerNews.Tests.Unit;

public class StoryServiceTests
{
    [Fact]
    public async Task GetNewestAsync_ReturnsPagedItems_AndTotal()
    {
        var client = new Mock<IHackerNewsClient>();
        client.Setup(c => c.GetNewestStoryIdsAsync(It.IsAny<CancellationToken>()))
              .ReturnsAsync(Enumerable.Range(1, 50).ToList());

        client.Setup(c => c.GetItemAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync((int id, CancellationToken _) => new HnItem { Id = id, Type = "story", Title = $"Story {id}", Url = $"https://x/{id}" });

        var cache = new MemoryCache(new MemoryCacheOptions());
        var svc = new StoryService(client.Object, cache);

        var result = await svc.GetNewestAsync(page: 2, pageSize: 10);

        result.Total.Should().Be(50);
        result.Items.Should().HaveCount(10);
        result.Items.First().Title.Should().Be("Story 11");
    }

    [Fact]
    public async Task SearchNewestAsync_FiltersByTitle_IgnoresUrlMissing()
    {
        var client = new Mock<IHackerNewsClient>();
        client.Setup(c => c.GetNewestStoryIdsAsync(It.IsAny<CancellationToken>()))
              .ReturnsAsync(new List<int> { 1, 2, 3 });

        client.Setup(c => c.GetItemAsync(1, It.IsAny<CancellationToken>()))
              .ReturnsAsync(new HnItem { Id = 1, Type = "story", Title = "Angular Tips", Url = null });
        client.Setup(c => c.GetItemAsync(2, It.IsAny<CancellationToken>()))
              .ReturnsAsync(new HnItem { Id = 2, Type = "story", Title = "React Guide", Url = "https://x/2" });
        client.Setup(c => c.GetItemAsync(3, It.IsAny<CancellationToken>()))
              .ReturnsAsync(new HnItem { Id = 3, Type = "story", Title = "Angular Testing", Url = "https://x/3" });

        var cache = new MemoryCache(new MemoryCacheOptions());
        var svc = new StoryService(client.Object, cache);

        var result = await svc.SearchNewestAsync("Angular", page: 1, pageSize: 10);

        result.Total.Should().Be(2);
        result.Items.Should().OnlyContain(s => s.Title.Contains("Angular"));
    }
}
