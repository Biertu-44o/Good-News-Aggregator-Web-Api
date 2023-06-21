using Core.DTOs.Article;
using Data.CQS.Commands;
using Entities_Context.Entities.UserNews;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.CQS.CommandsHandler
{
    public class RateArticlesCommandHandler : IRequestHandler<RateArticlesCommand>
    {
        private readonly UserArticleContext _articleContext;

        public RateArticlesCommandHandler(UserArticleContext articleContext)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));
        }

        public async Task Handle(RateArticlesCommand request, CancellationToken cancellationToken)
        {
            foreach (FullArticleDto article in request.Articles)
            {
                var articleToUpdate = await _articleContext.Articles
                    .Where(x => x.Id == article.Id)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                if (articleToUpdate != null)
                {
                    articleToUpdate.PositiveRate = article.PositiveRate;
                    articleToUpdate.FirstRate = article.FirstRate;
                    articleToUpdate.SecondRate = article.SecondRate;
                }
            }

            await _articleContext.SaveChangesAsync(cancellationToken);
        }
    }
}
