using AutoMapper;
using IServices;
using IServices.Services;

namespace MVC.ControllerFactory
{
    public interface IServiceFactory
    {
        IMapper CreateMapperService();
        IAuthService CreateIdentityService();
        IRoleService CreateRoleService();
        IUserService CreateAdminService();
        IConfiguration CreateConfigurationService();
        IArticleService CreateArticlesService();
        ISettingsService CreateUserConfigService();
        IUiThemeService CreateThemeService();
    }
   
}
