using FluentAssertions;
using FluentAssertions.Execution;
using HackerStories.Interfaces;
using HackerStories.Model;
using HackerStories.Services;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;

namespace HackerStoriesTests.Services
{
    public class HackerStoriesServiceTests
    {
        private readonly IHackerNewsClient _hackerNewsClient;
        private readonly IMapper<HackerStory, StoryDto> _mapper;
        private readonly IMemoryCache _cache;
        private readonly HackerStoriesService _hackerStoriesService;

        public HackerStoriesServiceTests() 
        {
            _hackerNewsClient = Substitute.For<IHackerNewsClient>();
            _mapper = Substitute.For<IMapper<HackerStory, StoryDto>>();
            _cache = Substitute.For<IMemoryCache>();
            _hackerStoriesService = new HackerStoriesService( _hackerNewsClient, _mapper, _cache);
        }

        [Fact]
        public async Task GetBestStoriesAsync_Success()
        {
            // Arrange
            var dateTime = DateTimeOffset.UtcNow;
            var unixTime = dateTime.ToUnixTimeSeconds();
            var story1Id = 11;
            HackerStory hackerStory1 = new HackerStory("Autor1", 5, 111, unixTime, "Title1","Link1");

            var story2Id = 22;
            HackerStory hackerStory2 = new HackerStory("Autor2", 52, 1112, unixTime, "Title2","Link2");

            _hackerNewsClient.ListBestStoriesAsync().Returns(new List<int>() { story1Id, story2Id });

            _cache.TryGetValue(story1Id, out Arg.Any<HackerStory>()).Returns(false);

            _hackerNewsClient.GetStoryAsync(story1Id).Returns(hackerStory1);

            StoryDto storyDto1 = new StoryDto(hackerStory1.Title, hackerStory1.Url, hackerStory1.By, dateTime.ToString(), hackerStory1.Score, hackerStory1.Descendants);
            _mapper.Map(hackerStory1).Returns(storyDto1);

            _cache.TryGetValue(story2Id, out Arg.Any<HackerStory>()).Returns(x =>
            {
                x[1] = hackerStory2;
                return true;
            });
            StoryDto storyDto2 = new StoryDto(hackerStory2.Title, hackerStory2.Url, hackerStory2.By, dateTime.ToString(), hackerStory2.Score, hackerStory2.Descendants);
            _mapper.Map(hackerStory2).Returns(storyDto2);

            // Act
            var results = await _hackerStoriesService.GetBestStoriesAsync(2);

            // Assert
            using (new AssertionScope())
            {
                await _hackerNewsClient.Received().ListBestStoriesAsync();
                _cache.Received(1).TryGetValue(story1Id, out Arg.Any<HackerStory>());
                await _hackerNewsClient.Received(1).GetStoryAsync(story1Id);
                _mapper.Received(1).Map(hackerStory1);
                _cache.Received(1).TryGetValue(story1Id, out Arg.Any<HackerStory>());
                _mapper.Received(1).Map(hackerStory2);
                results.Count().Should().Be(2);
                results[0].Should().BeEquivalentTo(storyDto1);
                results[1].Should().BeEquivalentTo(storyDto2);
            }
        }
    }
}