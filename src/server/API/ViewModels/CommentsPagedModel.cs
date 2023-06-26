using Models.SubModels.Message;

namespace API.ViewModels
{
    public class CommentsPagedModel
    {
        public IEnumerable<M_Comment> Comments { get; set; }
    }
}
