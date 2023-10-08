namespace Presentation.Models.ViewModels
{
    public record ProfileViewModel
    {
        public Guid ProfileId { get; set; }
        public string Tag { get; set; }
        public string FullName { get; set; }
        //public Photo ProfilePhoto { get; set; }
    }
}
