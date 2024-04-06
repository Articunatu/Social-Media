namespace SocialMedia.Domain.Abstractions
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
        DateTime? TimeOfDelete { get; set; }
    }
}