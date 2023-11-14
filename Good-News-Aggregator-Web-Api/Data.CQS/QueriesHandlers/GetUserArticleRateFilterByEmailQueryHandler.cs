using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.CQS.Queries;
using Entities_Context.Entities.UserNews;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Data.CQS.QueriesHandlers
{
    public class GetUserArticleRateFilterByEmailQueryHandler:IRequestHandler<GetUserArticleRateFilterByEmailQuery,Int32>
    {
        private readonly UserArticleContext _articleContext;

        public GetUserArticleRateFilterByEmailQueryHandler(UserArticleContext articleContext)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));
        }

        public async Task<Int32> Handle(GetUserArticleRateFilterByEmailQuery request, CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(request.Email))
            {
                throw new ArgumentNullException(nameof(request));
            }

            return await _articleContext.Users
                .AsNoTracking()
                .Where(x => x.Email.Equals(request.Email))
                .Select(x => x.PositiveRateFilter)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
