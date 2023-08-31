using HackerStories.Interfaces;
using HackerStories.Model;
using Microsoft.AspNetCore.Mvc;

namespace HackerStories.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class HackerStoriesController : ControllerBase
{
    private readonly ILogger<HackerStoriesController> _logger;
    private readonly IHackerStoriesService _hackerStoriesService;

    public HackerStoriesController(ILogger<HackerStoriesController> logger, IHackerStoriesService hackerStoriesService)
    {
        _logger = logger;
        _hackerStoriesService = hackerStoriesService;
    }

    [HttpGet("{numberOfStories}")]
    public async Task<IEnumerable<StoryDto>> GetBestStoriesAsync(int numberOfStories)
    {
        _logger.LogInformation($"Request of {numberOfStories} best stories");
        return await _hackerStoriesService.GetBestStoriesAsync(numberOfStories);
    }
}
