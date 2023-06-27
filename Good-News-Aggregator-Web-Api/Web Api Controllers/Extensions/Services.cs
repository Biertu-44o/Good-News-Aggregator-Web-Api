using IServices.Services;
using Services.Account;
using Services.Article;
using Services.Article.ArticleRate;
using Web_Api_Controllers.ControllerFactory;

namespace Web_Api_Controllers.Extensions
{
    public static class GoodNewsAggregatorServicesExtension
    {
        public static IServiceCollection AddGoodNewsAggregatorServices
            (this IServiceCollection services)
        {

            services.AddScoped<IServiceFactory, ServiceFactory>();
            services.AddScoped<ISentimentAnalyzerService, SentimentAnalyzerService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ISourceService, SourceService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUiThemeService, UiThemeService>();
            
            return services;
        }
    }
}
