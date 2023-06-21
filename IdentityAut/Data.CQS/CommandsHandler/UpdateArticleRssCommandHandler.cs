using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.DTOs.Article;
using Data.CQS.Commands;
using Entities_Context.Entities.UserNews;
using Hangfire.Dashboard;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Data.CQS.CommandsHandler
{
    public class UpdateArticleRssCommandHandler:IRequestHandler<UpdateArticleRssCommand>
    {
        private readonly UserArticleContext _articleContext;
        private readonly IMapper _mapper;

        public UpdateArticleRssCommandHandler(UserArticleContext articleContext, IMapper mapper)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));
            _mapper = mapper ?? throw new NullReferenceException(nameof(mapper));
        }

        public async Task Handle(UpdateArticleRssCommand request, CancellationToken cancellationToken)
        {

            foreach (FullArticleDto article in request.Articles)
            {
                var articleToUpdate = await _articleContext.Articles
                    .Where(x => x.Id == article.Id)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                if (articleToUpdate != null)
                {
                    articleToUpdate.ArticlePicture = article.ArticlePicture;
                    articleToUpdate.ShortDescription = article.ShortDescription;
                    articleToUpdate.FullText = article.FullText;
                    articleToUpdate.ArticleSourceUrl = article.ArticleSourceUrl;
                }
            }

            await _articleContext.SaveChangesAsync(cancellationToken);
        }
    }
}
