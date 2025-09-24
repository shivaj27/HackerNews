using System.Net.Http.Json;
using HackerNews.Models;

namespace HackerNews.Services;

public class HackerNewsClient : IHackerNewsClient
{
    private readonly HttpClient _http;
    public HackerNewsClient(HttpClient http) => _http = http;

    public async Task<IReadOnlyList<int>> GetNewestStoryIdsAsync(CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<IReadOnlyList<int>>("newstories.json", cancellationToken: ct)
        ?? Array.Empty<int>();

    public async Task<HnItem?> GetItemAsync(int id, CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<HnItem>($"item/{id}.json", cancellationToken: ct);
}
