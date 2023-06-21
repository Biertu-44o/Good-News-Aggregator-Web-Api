using AutoMapper;
using AutoMapper;
using Core.DTOs.Account;
using Core.DTOs.Article;
using Data.CQS.Commands;
using Entities_Context.Entities.UserNews;
using Web_Api_Controllers.RequestModels;
using Web_Api_Controllers.ResponseModels;
namespace Web_Api_Controllers.MappingProfiles
{
    public class AccountProfile: Profile
    {
        public AccountProfile()
        {
            SourceMemberNamingConvention = LowerUnderscoreNamingConvention.Instance;
            DestinationMemberNamingConvention = PascalCaseNamingConvention.Instance;

            CreateMap<UserLoginDto, LoginRequest>().ReverseMap();
            CreateMap<UserRegistrationDto, RegistrationRequest>().ReverseMap();
            CreateMap<User, UserRegistrationDto>().ReverseMap();

        }
    }
}
