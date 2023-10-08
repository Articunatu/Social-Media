using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Message : Entity
    {
        public Message(Guid id) : base(id) { }

        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
