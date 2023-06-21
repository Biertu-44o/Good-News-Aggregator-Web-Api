using Entities_Context.Entities.UserNews;
using IServices.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementations
{
    public class UserInterfaceThemeRepository : Repository<SiteTheme>, IUserInterfaceThemeRepository
    {
        public UserInterfaceThemeRepository(UserArticleContext newsAggregatorContext)
            : base(newsAggregatorContext)
        {
        }
    }
}
