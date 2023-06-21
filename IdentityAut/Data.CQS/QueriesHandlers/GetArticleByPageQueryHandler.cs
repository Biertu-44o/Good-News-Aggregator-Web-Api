using Core.DTOs.Article;
using Data.CQS.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Entities_Context.Entities.UserNews;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Data.CQS.QueriesHandlers
{
    public class GetArticleByPageQueryHandler: IRequestHandler<GetArticleByPageQuery, List<ShortArticleDto>>
    {
        private readonly UserArticleContext _articleContext;
        private  readonly IMapper _mapper;

        public GetArticleByPageQueryHandler(UserArticleContext articleContext, IMapper mapper)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));

            _mapper = mapper ?? throw new NullReferenceException(nameof(mapper));
        }

        public async Task<List<ShortArticleDto>> Handle(GetArticleByPageQuery request, CancellationToken cancellationToken)
        {

            return await _articleContext.Articles
                .AsNoTracking()
                .Where(x => request.UserFilter <= x.PositiveRate)
                .OrderByDescending(x=>x.DateTime)
                .Include(x=>x.Source)
                .Skip((request.Page-1)*request.Count)
                .Take(request.Count)
                .Select(a => _mapper
                    .Map<ShortArticleDto>(a))
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
