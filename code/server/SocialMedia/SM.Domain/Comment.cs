
namespace SM.Domain
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int? ParentId { get; set; }  
        public List<Comment> Replies { get; set; } = [];
    }

}
