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
    public class GetArticlesHashIdBySourceIdQueryHandler:IRequestHandler<GetArticlesHashIdBySourceIdQuery,List<String>>
    {
        private readonly UserArticleContext _articleContext;

        public GetArticlesHashIdBySourceIdQueryHandler(UserArticleContext articleContext)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));
        }

        public async Task<List<String>> Handle(GetArticlesHashIdBySourceIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
            {
                throw new ArgumentNullException(nameof(request));
            }
            return await _articleContext.Articles
                .Where(x => x.SourceId == request.Id)
                .Select(x=>x.HashUrlId)
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
