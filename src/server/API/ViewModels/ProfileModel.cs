namespace API.ViewModels
{
    public record ProfileModel
    {
        public string? Tag { get; set; }
        public string? Fullname { get; set; }
        public string? ProfilePicture { get; set; }
    }
}