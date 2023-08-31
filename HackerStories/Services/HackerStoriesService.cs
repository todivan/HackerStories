using HackerStories.Interfaces;
using HackerStories.Model;
using Microsoft.Extensions.Caching.Memory;
using RestSharp;

namespace HackerStories.Services;

public class HackerStoriesService : IHackerStoriesService
{
    private readonly IHackerNewsClient _hackerNewsClient;
    private readonly IMapper<HackerStory, StoryDto> _mapper;
    private readonly IMemoryCache _cache;

    public HackerStoriesService(IHackerNewsClient hackerNewsClient, IMapper<HackerStory, StoryDto> mapper, IMemoryCache cache)
    {
        _hackerNewsClient = hackerNewsClient;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<List<StoryDto>> GetBestStoriesAsync(int numberOfStories)
    {
        var bestStoriesIds = await _hackerNewsClient.ListBestStoriesAsync();

        var bestStories = new List<StoryDto>();

        foreach (var storyId in bestStoriesIds)
        {
            var story = await GetStoryAsync(storyId);

            var storyDto = _mapper.Map(story);
            bestStories.Add(storyDto);

            if(bestStories.Count() >= numberOfStories)
            {
                break;
            }
        }

        return bestStories;
    }

    private async Task<HackerStory> GetStoryAsync(int storyId)
    {
        HackerStory story = default!;

        if(!_cache.TryGetValue(storyId, out story))
        {
            story = await _hackerNewsClient.GetStoryAsync(storyId);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(600))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                .SetPriority(CacheItemPriority.Normal);

            _cache.Set(storyId, story, cacheEntryOptions);
        }

        return story;
    }
}

