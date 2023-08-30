using HackerStories.Model;

namespace HackerStories.Interfaces; 

public interface IHackerStoriesService 
{
    public Task<List<StoryDto>> GetBestStoriesAsync(int numberOfStories);
}
