using HackerNews.Models;
using HackerNews.Services;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HackerNewsController : ControllerBase
{
    private readonly IStoryService _stories;
    public HackerNewsController(IStoryService stories) => _stories = stories;

    [HttpGet("newest")]
    public async Task<ActionResult<PagedResult<StoryDto>>> Newest(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        if (page < 1 || pageSize is < 1 or > 100) return BadRequest("Invalid page or pageSize.");
        return Ok(await _stories.GetNewestAsync(page, pageSize, ct));
    }

    [HttpGet("search")]
    public async Task<ActionResult<PagedResult<StoryDto>>> Search(
        [FromQuery] string query = "",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        if (page < 1 || pageSize is < 1 or > 100) return BadRequest("Invalid page or pageSize.");
        return Ok(await _stories.SearchNewestAsync(query, page, pageSize, ct));
    }
}
