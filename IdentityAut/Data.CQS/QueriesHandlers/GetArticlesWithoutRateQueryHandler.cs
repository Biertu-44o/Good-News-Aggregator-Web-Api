using AutoMapper;
using Core.DTOs.Article;
using Data.CQS.Queries;
using Entities_Context.Entities.UserNews;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Data.CQS.QueriesHandlers
{

    public class GetArticlesWithoutRateQueryHandler : IRequestHandler<GetArticlesWithoutRateQuery, List<FullArticleDto>>
    {
        private readonly UserArticleContext _articleContext;
        private readonly IMapper _mapper;
        public GetArticlesWithoutRateQueryHandler(UserArticleContext articleContext, IMapper mapper)
        {
            _mapper = mapper ?? throw new NullReferenceException(nameof(mapper));

            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));
        }

        public async Task<List<FullArticleDto>> Handle(GetArticlesWithoutRateQuery request, CancellationToken cancellationToken)
        {
            return await _articleContext.Articles
                .Where(x => x.FullText !=null)
                .Where(x => x.PositiveRate==0)
                .Select(x => _mapper.Map<FullArticleDto>(x))
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
