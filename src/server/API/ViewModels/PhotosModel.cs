using Core.Paging;
using Models.SubModels;

namespace API.ViewModels
{
    public class PhotosModel
    {
        public PagedResult<Photo> Photos { get; set; }
    }
}
