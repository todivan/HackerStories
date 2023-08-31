using HackerStories.Interfaces;
using HackerStories.Model;
using RestSharp;

namespace HackerStories.Services;

public class HackerNewsClient : IHackerNewsClient
{
    private readonly IRestClient _restClient;

    public HackerNewsClient(IRestClient restClient)
    {
        _restClient = restClient;
    }

    public async Task<List<int>> ListBestStoriesAsync()
    {
        List<int> list = new List<int>();

        RestRequest restRequest= new RestRequest("beststories.json", Method.Get);
        restRequest.AddHeader("content-type", "application/json");

        var respone = await _restClient.ExecuteAsync<List<int>>(restRequest);
        if(respone.IsSuccessStatusCode && respone.Data != null)
        {
            list = respone.Data; 
        }

        return list;
    }

    public async Task<HackerStory> GetStoryAsync(int storyId)
    {
        HackerStory hackerStory = default!;

        RestRequest restRequest = new RestRequest($"/item/{storyId}.json", Method.Get);
        restRequest.AddHeader("content-type", "application/json");

        var respone = await _restClient.ExecuteAsync<HackerStory>(restRequest);
        if (respone.IsSuccessStatusCode && respone.Data != null)
        {
            hackerStory = respone.Data;
        }

        return hackerStory;
    }
}

