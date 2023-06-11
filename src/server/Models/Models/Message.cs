using Models.DataTransferObjects;
using Models.SubModels.Message;

namespace Models.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public AccountDto Account { get; set; }
        public ICollection<MessageReaction>? Reactions { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
