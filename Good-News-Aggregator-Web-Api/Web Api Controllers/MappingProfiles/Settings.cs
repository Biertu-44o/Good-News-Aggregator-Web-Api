using AutoMapper;
using Core.DTOs.Account;
using Entities_Context.Entities.UserNews;
using Web_Api_Controllers.RequestModels;
using Web_Api_Controllers.ResponseModels;

namespace Web_Api_Controllers.MappingProfiles
{
    public class SettingsProfile:Profile
    {
        public SettingsProfile()
        {
            SourceMemberNamingConvention = LowerUnderscoreNamingConvention.Instance;
            DestinationMemberNamingConvention = PascalCaseNamingConvention.Instance;

            CreateMap<userSettingsDTO, User>().ReverseMap().ForMember(
                dest => dest.Theme,
                opt =>
                    opt.MapFrom(src => src.Theme.Theme)
            );

            CreateMap<userSettingsDTO, PutSettingsRequest>().ReverseMap();
        }
    }

}
