using HackerNews.Models;

namespace HackerNews.Services;

public interface IStoryService
{
    Task<PagedResult<StoryDto>> GetNewestAsync(int page, int pageSize, CancellationToken ct = default);
    Task<PagedResult<StoryDto>> SearchNewestAsync(string query, int page, int pageSize, CancellationToken ct = default);
}
