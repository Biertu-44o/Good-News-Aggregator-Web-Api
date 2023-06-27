using Data.CQS.Queries;
using Entities_Context.Entities.UserNews;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.DTOs.Article;
using Microsoft.EntityFrameworkCore;

namespace Data.CQS.QueriesHandlers
{

    public class GetCommentsByArticleIdQueryHandler : IRequestHandler<GetCommentsByArticleIdQuery, List<CommentDto>>
    {
        private readonly UserArticleContext _articleContext;
        private readonly IMapper _mapper;


        public GetCommentsByArticleIdQueryHandler(UserArticleContext articleContext, IMapper mapper)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));

            _mapper = mapper ?? throw new NullReferenceException(nameof(mapper));
        }

        public async Task<List<CommentDto>> Handle(GetCommentsByArticleIdQuery request, CancellationToken cancellationToken)
        {
            if (request.articleId==0)
            {
                throw new ArgumentNullException(nameof(request.articleId));
            }
            return await _articleContext.Comments
                .AsNoTracking()
                .Where(x => x.ArticleId == request.articleId)
                .Include(x=>x.User)
                .Select(x => _mapper.Map<CommentDto>(x))
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }

}
