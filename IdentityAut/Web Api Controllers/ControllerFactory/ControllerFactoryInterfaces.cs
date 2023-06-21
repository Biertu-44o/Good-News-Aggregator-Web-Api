using AutoMapper;
using FluentValidation;
using IServices.Services;
using Web_Api_Controllers.RequestModels;

namespace Web_Api_Controllers.ControllerFactory
{
    public interface IServiceFactory
    {
        IMapper CreateMapperService();
        ICommentService CreateCommentService();
        IUserService CreateIdentityService();
        IRoleService CreateRoleService();
        IConfiguration CreateConfigurationService();
        IArticleService CreateArticlesService();
        ISettingsService CreateUserConfigService();
        IUiThemeService CreateThemeService();
        IJwtService CreateJwtService();
        IValidator<LoginRequest> CreateLoginValidator();
        IValidator<RegistrationRequest> CreateRegistrationValidator();
        IValidator<GetArticlesRequest> CreatePageValidator();
        IValidator<PutSettingsRequest> CreateSettingsValidator();
    }
   
}
