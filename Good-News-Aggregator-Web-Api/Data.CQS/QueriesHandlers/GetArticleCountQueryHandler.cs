using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Data.CQS.Queries;
using Entities_Context.Entities.UserNews;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Data.CQS.QueriesHandlers
{
    public class GetArticleCountQueryHandler : IRequestHandler<GetArticleCountQuery,Int32>
    {
        private readonly UserArticleContext _articleContext;

        public GetArticleCountQueryHandler(UserArticleContext articleContext)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));
        }

        public async Task<Int32> Handle(GetArticleCountQuery request, CancellationToken cancellationToken)
        {
            return await _articleContext.Articles.AsNoTracking()
                .Where(x => x.PositiveRate >= request.UserRateFilter)
                .CountAsync(cancellationToken: cancellationToken);
        }
    }
}
