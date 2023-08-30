using HackerStories.Interfaces;
using HackerStories.Model;

namespace HackerStories.Mappers;

public class HackerStoryToStoryDtoMapper : IMapper<HackerStory, StoryDto>
{
    public StoryDto Map(HackerStory hackerStory)
    {
        var time = DateTimeOffset.FromUnixTimeSeconds(hackerStory.Time).ToString("yyyy-MM-ddTHH:mm:sszzz");
        return new StoryDto(hackerStory.Title, hackerStory.Url, hackerStory.By, time, hackerStory.Score, hackerStory.Descendants);
    }
}

