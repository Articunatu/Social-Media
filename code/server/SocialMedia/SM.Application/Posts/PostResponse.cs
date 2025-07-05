namespace SM.Application.Posts;

public class PostResponse
{
    public PostResponse() { }

    public PostResponse(Guid id, string content, Guid userId, DateTimeOffset timeStamp)
    {
        Id = id;
        Content = content;
        UserId = userId;
        TimeStamp = timeStamp;
    }

    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset TimeStamp { get; set; }
    public Guid UserId { get; set; }
}