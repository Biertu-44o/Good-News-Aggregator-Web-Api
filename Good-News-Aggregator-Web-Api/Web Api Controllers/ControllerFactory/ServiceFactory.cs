using AutoMapper;
using FluentValidation;
using IServices.Services;
using Web_Api_Controllers.RequestModels;
using Web_Api_Controllers.Validators;

namespace Web_Api_Controllers.ControllerFactory
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? 
                               throw new NullReferenceException(nameof(serviceProvider));
        }


        IArticleService IServiceFactory.CreateArticlesService()
        {
            return _serviceProvider.GetService<IArticleService>()
                   ?? throw new NullReferenceException(nameof(IArticleService));
        }
        ICommentService IServiceFactory.CreateCommentService()
        {
            return _serviceProvider.GetService<ICommentService>()
                   ?? throw new NullReferenceException(nameof(ICommentService));
        }
        IValidator<GetArticlesRequest> IServiceFactory.CreatePageValidator()
        {
            return _serviceProvider.GetService<IValidator<GetArticlesRequest>>()
                   ?? throw new NullReferenceException(nameof(IValidator<GetArticlesRequest>));
        }

        IValidator<PutSettingsRequest> IServiceFactory.CreateSettingsValidator()
        {
            return _serviceProvider.GetService<IValidator<PutSettingsRequest>>()
                   ?? throw new NullReferenceException(nameof(IValidator<PutSettingsRequest>));
        }

        IConfiguration IServiceFactory.CreateConfigurationService()
        {
            return _serviceProvider.GetService<IConfiguration>()
                   ?? throw new NullReferenceException(nameof(IConfiguration));
        }

        IUserService IServiceFactory.CreateIdentityService()
        {
            return _serviceProvider.GetService<IUserService>()
                   ?? throw new NullReferenceException(nameof(IUserService));
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
        IJwtService IServiceFactory.CreateJwtService()
        {
            return _serviceProvider.GetService<IJwtService>()
                   ?? throw new NullReferenceException(nameof(ISettingsService));
        }

        IValidator<LoginRequest> IServiceFactory.CreateLoginValidator()
        {
            return _serviceProvider.GetService<IValidator<LoginRequest>>()
                   ?? throw new NullReferenceException(nameof(IValidator<LoginRequest>));
        }
        
        IValidator<RegistrationRequest> IServiceFactory.CreateRegistrationValidator()
        {
            return _serviceProvider.GetService<IValidator<RegistrationRequest>>()
                   ?? throw new NullReferenceException(nameof(IValidator<RegistrationRequest>));
        }
    }
}
