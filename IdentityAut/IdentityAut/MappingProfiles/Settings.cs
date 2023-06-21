using AutoMapper;
using Core.DTOs.Account;
using Entities_Context.Entities.UserNews;
using MVC.Models.UserSettings;

namespace MVC.MappingProfiles
{
    public class SettingsProfile:Profile
    {
        public SettingsProfile()
        {
            SourceMemberNamingConvention = LowerUnderscoreNamingConvention.Instance;
            DestinationMemberNamingConvention = PascalCaseNamingConvention.Instance;

            CreateMap<userInfoWithSettingsDTO, User>().ReverseMap();

            CreateMap<userInfoWithSettingsDTO, NewUserSettingsViewModel>().ReverseMap();

            CreateMap<ShowUserInfoAndConfigViewModel, userInfoWithSettingsDTO>().ReverseMap();
        }
    }

}
