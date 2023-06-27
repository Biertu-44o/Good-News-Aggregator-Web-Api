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
    public class GetAllThemesQueryHandler:IRequestHandler<GetAllThemesQuery,List<String>>
    {
        private readonly UserArticleContext _articleContext;

        public GetAllThemesQueryHandler(UserArticleContext articleContext)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));
        }

        public async Task<List<String>> Handle(GetAllThemesQuery request, CancellationToken cancellationToken)
        {
            return await _articleContext.Themes
                .AsNoTracking()
                .Select(x=>x.Theme)
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
