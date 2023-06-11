using Models.DataTransferObjects;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.SubModels.Message
{
    public class MessageReaction
    {
        public AccountDto Reactor { get; set; }
        public ReactionType Reaction { get; set; }
    }
}
