using AutoMapper;
using Core.DTOs.Account;
using Entities_Context.Entities.UserNews;
using MVC.Models.AccountModels;

namespace MVC.MappingProfiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            SourceMemberNamingConvention = LowerUnderscoreNamingConvention.Instance;
            DestinationMemberNamingConvention = PascalCaseNamingConvention.Instance;
            
            CreateMap<User, UserRegistrationDto>().ReverseMap();
            CreateMap<UserRegistrationViewModel, UserRegistrationDto>().ReverseMap();

            CreateMap<User, UserLoginDto>().ReverseMap();
            CreateMap<UserLoginViewModel, UserLoginDto>().ReverseMap();

            CreateMap<User, UserDto>();
        }
    }
}
