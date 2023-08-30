namespace HackerStories.Model;

public sealed record StoryDto(
    string Title,
    string Uri,
    string PostedBy,
    string Time,
    int Score,
    int CommentCount);
