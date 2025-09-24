using HackerNews.Models;

namespace HackerNews.Services;

public interface IHackerNewsClient
{
    Task<IReadOnlyList<int>> GetNewestStoryIdsAsync(CancellationToken ct = default);
    Task<HnItem?> GetItemAsync(int id, CancellationToken ct = default);
}
