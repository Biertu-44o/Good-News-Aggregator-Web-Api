using IServices.Repositories;
using IServices;
using IServices.Services;
using Repositories.Implementations;
using Repositories;
using Services.Account;
using Services.Article;

namespace MVC.Extensions
{
    public static class GoodNewsAggregatorRepositoriesExtension
    {
        public static IServiceCollection AddGoodNewsAggregatorRepositories
            (this IServiceCollection repositories)
        {
            repositories.AddScoped<IUnitOfWork, UnitOfWork>();
            repositories.AddScoped<IArticleRepository, ArticleRepository>();
            repositories.AddScoped<ITagRepository, TagRepository>();
            repositories.AddScoped<IUsersRepository, UsersRepository>();
            repositories.AddScoped<ISourceRepository, SourceRepository>();
            repositories.AddScoped<IRoleRepository, RoleRepository>();
            repositories.AddScoped<IUsersRolesRepository, UsersRolesRepository>();
            repositories.AddScoped<IArticleTagRepository, ArticleTagRepository>();
            repositories.AddScoped<IUserInterfaceThemeRepository, UserInterfaceThemeRepository>();
            
            return repositories;
        }
    }
}
