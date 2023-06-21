using Models.DataTransferObjects;
using Models.SubModels.Message;
using Newtonsoft.Json;

namespace Models.Models
{
    public class Message
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public AccountDto Account { get; set; }
        public ICollection<MessageReaction>? Reactions { get; set; }
        public ICollection<M_Comment>? Comments { get; set; }
    }
}
