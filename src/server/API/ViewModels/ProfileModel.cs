using Models.SubModels;

namespace API.ViewModels
{
    public record ProfileModel
    {
        public string? Tag { get; set; }
        public string? FullName { get; set; }
        public Photo? ProfilePicture { get; set; }
    }
}