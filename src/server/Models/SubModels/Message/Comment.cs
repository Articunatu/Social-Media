using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.SubModels.Message
{
    public class Comment
    {
        public Guid Id { get; set; }
        public AccountDto Author { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<MessageReaction>? Reactions { get; set; }
    }
}
