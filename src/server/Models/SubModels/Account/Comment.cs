using Models.SubModels.Message;

namespace Models.SubModels.Account
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Post Post { get; set; }
        public string? Text { get; set; }
        public DateTime Date { get; set; }
        public ICollection<MessageReaction>? Reactions { get; set; }
    }
}
