using HackerNews.Models;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNews.Services;

public class StoryService : IStoryService
{
    private readonly IHackerNewsClient _client;
    private readonly IMemoryCache _cache;

    private const string NewIdsCacheKey = "hn:newstories";
    private static readonly MemoryCacheEntryOptions IdsCacheOpts =
        new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2) };
    private static readonly MemoryCacheEntryOptions ItemCacheOpts =
        new() { SlidingExpiration = TimeSpan.FromMinutes(5) };

    public StoryService(IHackerNewsClient client, IMemoryCache cache)
    {
        _client = client;
        _cache = cache;
    }

    public async Task<PagedResult<StoryDto>> GetNewestAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var ids = await GetCachedNewIds(ct);
        var total = ids.Count;
        var slice = ids.Skip((page - 1) * pageSize).Take(pageSize).ToArray();
        var items = await FetchStories(slice, ct);

        return new PagedResult<StoryDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<PagedResult<StoryDto>> SearchNewestAsync(string query, int page, int pageSize, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(query)) return await GetNewestAsync(page, pageSize, ct);

        var ids = (await GetCachedNewIds(ct)).Take(400).ToArray();
        var all = await FetchStories(ids, ct);

        var filtered = all
            .Where(s => s.Title.Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var total = filtered.Count;
        var paged = filtered.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return new PagedResult<StoryDto>
        {
            Items = paged,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    private async Task<IReadOnlyList<int>> GetCachedNewIds(CancellationToken ct)
    {
        if (_cache.TryGetValue(NewIdsCacheKey, out IReadOnlyList<int> cached)) return cached;
        var ids = await _client.GetNewestStoryIdsAsync(ct);
        _cache.Set(NewIdsCacheKey, ids, IdsCacheOpts);
        return ids;
    }

    private async Task<IReadOnlyList<StoryDto>> FetchStories(IEnumerable<int> ids, CancellationToken ct)
    {
        var tasks = ids.Select(async id =>
        {
            var key = $"hn:item:{id}";
            if (_cache.TryGetValue(key, out StoryDto? cached)) return cached;

            var item = await _client.GetItemAsync(id, ct);
            if (item is null || !string.Equals(item.Type, "story", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(item.Title))
                return null;

            var dto = new StoryDto(item.Id, item.Title!, item.Url);
            _cache.Set(key, dto, ItemCacheOpts);
            return dto;
        });

        var results = await Task.WhenAll(tasks);
        return results.Where(x => x is not null).Select(x => x!).ToList();
    }
}
