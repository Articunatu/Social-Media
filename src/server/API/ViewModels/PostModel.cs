using AutoMapper;

namespace API.ViewModels
{
    public class PostModel
    {
        public ProfileModel Profile;
        public string Text;
        public DateTime Date;

        public MultipleReactionsVM Reactions;
        public int CommentsAmt;
    }
}