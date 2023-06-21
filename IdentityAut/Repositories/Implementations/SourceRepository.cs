using Entities_Context.Entities.UserNews;
using IServices.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementations
{
    public class SourceRepository : Repository<Source>, ISourceRepository
    {
        public SourceRepository(UserArticleContext newsAggregatorContext)
            : base(newsAggregatorContext)
        {
        }

    }
}
