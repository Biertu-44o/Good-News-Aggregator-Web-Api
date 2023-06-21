using System.Data;
using Entities_Context.Entities.UserNews;

namespace Abstract
{
    public interface UnitOfWork : IDisposable
    {
        public IRepository<Article> Articles { get; }
        public IRepository<Comment> Comments { get; }
        public IRepository<ArticleTag> ArticleTeg { get; }
        public IRepository<UserRole> Roles { get; }
        public IRepository<Source> Source { get; }
        public IRepository<Tag> Teg { get; }
        public IRepository<SiteTheme> Theme { get; }
        public IRepository<User> User { get; }
        public IRepository<UsersRoles> UserRoles { get; }
        public Task<int> SaveChangesAsync();
    }
}