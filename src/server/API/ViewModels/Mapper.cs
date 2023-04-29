using Models.Models;
using Models.SubModels.Account;

namespace API.ViewModels
{
    public class Mapper : AutoMapper.Profile
    {
        public Mapper()
        {
            CreateMap<Account, ProfileModel>()
            .ForMember(dest => dest.Tag, src => src.MapFrom(t => t.Tag))
                .ForMember(dest => dest.ProfilePicture, src => src.MapFrom(t => t.Photos.Where(p => p.IsProfilePhoto)))
                .ForMember(dest => dest.Fullname, src => src.MapFrom(t => t.FirstName + " " + t.LastName));

            CreateMap<Message, PostModel>()
                .ForMember(dest => dest.Text, src => src.MapFrom(t => t.Text))
                .ForMember(dest => dest.Date, src => src.MapFrom(t => t.Date))
                .ForMember(dest => dest.Profile, src => src.MapFrom(t => t.Account));
        }
    }
}
