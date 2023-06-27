using Data.CQS.Commands;
using Entities_Context.Entities.UserNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;

namespace Data.CQS.CommandsHandler
{
    internal class AddFullArticlesCommandHandler:IRequestHandler<AddFullArticlesCommand>
    {
        private readonly UserArticleContext _articleContext;
        private readonly IMapper _mapper;

        public AddFullArticlesCommandHandler(UserArticleContext articleContext, IMapper mapper)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));

            _mapper = mapper ?? throw new NullReferenceException(nameof(mapper));
        }

        public async Task Handle(AddFullArticlesCommand request,
            CancellationToken cancellationToken)
        {
            List<Article> articles = request.Articles.Select(a=>_mapper.Map<Article>(a)).ToList();
            
            await _articleContext.Articles.AddRangeAsync(articles, cancellationToken);
            
            await _articleContext.SaveChangesAsync(cancellationToken);
        }
    }
}
