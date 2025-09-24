using System.Net;
using System.Net.Http.Json;
using HackerNews.Models;
using HackerNews.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using FluentAssertions;

namespace HackerNews.Tests.Integration;

public class HackerNewsEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public HackerNewsEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.Single(d => d.ServiceType == typeof(IHackerNewsClient));
                services.Remove(descriptor);

                var mock = new Mock<IHackerNewsClient>();
                mock.Setup(c => c.GetNewestStoryIdsAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Enumerable.Range(1, 30).ToList());
                mock.Setup(c => c.GetItemAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((int id, CancellationToken _) =>
                        new HnItem { Id = id, Type = "story", Title = $"Title {id}", Url = $"https://x/{id}" });

                services.AddSingleton<IHackerNewsClient>(mock.Object);
            });
        });
    }

    [Fact]
    public async Task Newest_ReturnsOk_WithPagedPayload()
    {
        var client = _factory.CreateClient();
        var resp = await client.GetAsync("/api/hackernews/newest?page=1&pageSize=5");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await resp.Content.ReadFromJsonAsync<PagedResult<StoryDto>>();
        payload!.Items.Should().HaveCount(5);
        payload.Total.Should().Be(30);
    }
}
