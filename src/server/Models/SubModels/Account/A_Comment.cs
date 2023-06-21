using Models.SubModels.Message;

namespace Models.SubModels.Account
{
    public class A_Comment
    {
        public Guid Id { get; set; }
        public Post Post { get; set; }
        public string? Text { get; set; }
        public DateTime Date { get; set; }
        public ICollection<MessageReaction>? Reactions { get; set; }

        public A_Comment(Guid id, Post post, string? text, DateTime date, ICollection<MessageReaction>? reactions)
        {
            Id = id;
            Post = post;
            Text = text;
            Date = date;
            Reactions = reactions;
        }
    }
}
