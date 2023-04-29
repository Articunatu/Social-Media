using Models.SubModels.Message;

namespace Models.SubModels.Account
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public IEnumerable<MessageReaction>? Reactions { get; set; }
        public IEnumerable<Comment>? Comments { get; set; }
    }
}
