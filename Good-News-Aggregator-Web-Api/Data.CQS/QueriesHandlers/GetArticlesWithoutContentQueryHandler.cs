using Data.CQS.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Article;
using Entities_Context.Entities.UserNews;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;

namespace Data.CQS.QueriesHandlers
{
    public class GetArticlesWithoutContentQueryHandler: IRequestHandler<GetArticlesWithoutContentQuery, List<FullArticleDto>>
    {
        private readonly UserArticleContext _articleContext;
        private readonly IMapper _mapper;
        public GetArticlesWithoutContentQueryHandler(UserArticleContext articleContext, IMapper mapper)
        { 
            _mapper = mapper ?? throw new NullReferenceException(nameof(mapper));

            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));
        }

        public async Task<List<FullArticleDto>> Handle(GetArticlesWithoutContentQuery request, CancellationToken cancellationToken)
        {
            return await _articleContext.Articles
                .AsNoTracking()
                .Where(x => x.FullText==null)
                .Include(x=>x.Source)
                .Select(x=>_mapper.Map<FullArticleDto>(x))
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
