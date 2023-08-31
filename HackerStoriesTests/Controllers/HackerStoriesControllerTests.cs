using FluentAssertions;
using FluentAssertions.Execution;
using HackerStories.Controllers;
using HackerStories.Interfaces;
using HackerStories.Model;
using Microsoft.Extensions.Logging;
using NSubstitute;


namespace HackerStoriesTests.Controllers
{
    public class HackerStoriesControllerTests
    {
        private readonly ILogger<HackerStoriesController> _logger;
        private readonly IHackerStoriesService _hackerStoriesService;
        private readonly HackerStoriesController _hackerStoriesController;

        public HackerStoriesControllerTests()
        {
            _logger = Substitute.For<ILogger<HackerStoriesController>>();
            _hackerStoriesService = Substitute.For<IHackerStoriesService>();
            _hackerStoriesController = new HackerStoriesController(_logger, _hackerStoriesService);
        }

        [Fact]
        public async Task GetBestStoriesAsync_Success()
        {
            // Arrange
            StoryDto storyDto1 = new StoryDto("Title", "Link", "Autor", "DateTime", 111, 5);
            StoryDto storyDto2 = new StoryDto("Title2", "Link2", "Autor2", "DateTime2", 1112, 52);

            _hackerStoriesService.GetBestStoriesAsync(2).Returns(new List<StoryDto>() { storyDto1, storyDto2 });


            // Act
            var results = await _hackerStoriesController.GetBestStoriesAsync(2);

            // Assert
            using (new AssertionScope())
            {
                await _hackerStoriesService.Received().GetBestStoriesAsync(2);
                results.Count().Should().Be(2);
                results.ElementAt(0).Should().BeEquivalentTo(storyDto1);
                results.ElementAt(1).Should().BeEquivalentTo(storyDto2);
            }
        }
    }
}
