using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.DTOs.Article;
using Data.CQS.Queries;
using Entities_Context.Entities.UserNews;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Data.CQS.QueriesHandlers
{
    public class GetFullArticleByIdQueryHandler:IRequestHandler<GetFullArticleByIdQuery,FullArticleDto?>
    {
        private readonly UserArticleContext _articleContext;
        private readonly IMapper _mapper;

        public GetFullArticleByIdQueryHandler(UserArticleContext articleContext, IMapper mapper)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));

            _mapper = mapper ?? throw new NullReferenceException(nameof(mapper));
        }

        public async Task<FullArticleDto?> Handle(GetFullArticleByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
            {
                throw new ArgumentNullException(nameof(request));
            }

            FullArticleDto? article=(await _articleContext.Articles
                .AsNoTracking()
                .Where(x => request.Id == x.Id)
                .Include(x => x.Source)
                .Select(a => _mapper
                    .Map<FullArticleDto>(a))
                .FirstOrDefaultAsync(cancellationToken: cancellationToken));

            return article;
            
        }
    }
}
