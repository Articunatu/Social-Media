using Models.DataTransferObjects;

namespace API.ViewModels
{
    public class FollowingModel
    {
        public IEnumerable<AccountDto> Following { get; set; }
    }
}
