﻿using HackerStories.Model;

namespace HackerStories.Interfaces;
public interface IHackerNewsClient
{
    public Task<List<int>> ListBestStoriesAsync();
    public Task<HackerStory> GetStoryAsync(int storyId);
}

