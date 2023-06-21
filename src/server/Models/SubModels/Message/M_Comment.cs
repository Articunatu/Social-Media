using Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.SubModels.Message
{
    public class M_Comment
    {
        public Guid Id { get; set; }
        public AccountDto Author { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public ICollection<MessageReaction>? Reactions { get; set; }

        public M_Comment(Guid id, AccountDto author, string text, DateTime date, ICollection<MessageReaction>? reactions)
        {
            Id = id;
            Author = author;
            Text = text;
            Date = date;
            Reactions = reactions;
        }
    }
}
