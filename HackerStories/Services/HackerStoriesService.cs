using HackerStories.Interfaces;
using HackerStories.Model;
using RestSharp;

namespace HackerStories.Services;

public class HackerStoriesService : IHackerStoriesService
{
    private readonly IHackerNewsClient _hackerNewsClient;
    private readonly IMapper<HackerStory, StoryDto> _mapper;

    public HackerStoriesService(IHackerNewsClient hackerNewsClient, IMapper<HackerStory, StoryDto> mapper)
    {
        _hackerNewsClient = hackerNewsClient;
        _mapper = mapper;
    }

    public async Task<List<StoryDto>> GetBestStoriesAsync(int numberOfStories)
    {
        var bestStoriesIds = await _hackerNewsClient.ListBestStoriesAsync();

        var bestStories = new List<StoryDto>();

        foreach (var storyId in bestStoriesIds)
        {
            var story = await _hackerNewsClient.GetStoryAync(storyId);
            
            var storyDto = _mapper.Map(story);
            bestStories.Add(storyDto);

            if(bestStories.Count() >= numberOfStories)
            {
                break;
            }
        }

        return bestStories;
    }
}

