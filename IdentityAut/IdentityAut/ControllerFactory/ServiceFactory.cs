using AutoMapper;
using IServices;
using IServices.Services;

namespace MVC.ControllerFactory
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        IUserService IServiceFactory.CreateAdminService()
        {
            return _serviceProvider.GetService<IUserService>() 
                   ?? throw new NullReferenceException(nameof(IUserService));
        }

        IArticleService IServiceFactory.CreateArticlesService()
        {
            return _serviceProvider.GetService<IArticleService>()
                   ?? throw new NullReferenceException(nameof(IArticleService));
        }

        IConfiguration IServiceFactory.CreateConfigurationService()
        {
            return _serviceProvider.GetService<IConfiguration>()
                   ?? throw new NullReferenceException(nameof(IConfiguration));
        }

        IAuthService IServiceFactory.CreateIdentityService()
        {
            return _serviceProvider.GetService<IAuthService>()
                   ?? throw new NullReferenceException(nameof(IAuthService));
        }

        IMapper IServiceFactory.CreateMapperService()
        {
            return _serviceProvider.GetService<IMapper>()
                   ?? throw new NullReferenceException(nameof(IMapper));
        }

        IRoleService IServiceFactory.CreateRoleService()
        {
            return _serviceProvider.GetService<IRoleService>()
                   ?? throw new NullReferenceException(nameof(IRoleService));
        }

        IUiThemeService IServiceFactory.CreateThemeService()
        {
            return _serviceProvider.GetService<IUiThemeService>()
                   ?? throw new NullReferenceException(nameof(IUiThemeService));
        }

        ISettingsService IServiceFactory.CreateUserConfigService()
        {
            return _serviceProvider.GetService<ISettingsService>()
                   ?? throw new NullReferenceException(nameof(ISettingsService));
        }
    }
}
