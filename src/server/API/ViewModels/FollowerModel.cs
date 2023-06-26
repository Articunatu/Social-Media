using Models.DataTransferObjects;

namespace API.ViewModels
{
    public class FollowerModel
    {
        public IEnumerable<AccountDto> Followers { get; set; }
    }
}
