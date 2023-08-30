namespace HackerStories.Model;

public sealed record HackerStory(
    string By,
    int Descendants,
    int Score,
    long Time,
    string Title,
    string Url);

