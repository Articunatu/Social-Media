using Models.SubModels;

namespace Models.DataTransferObjects
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string Tag { get; set; }
        public string FullName { get; set; }
        public Photo? ProfilePhoto { get; set; }
    }
}
