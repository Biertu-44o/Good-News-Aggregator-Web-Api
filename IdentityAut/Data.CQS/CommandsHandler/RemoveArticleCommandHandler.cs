using Entities_Context.Entities.UserNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.CQS.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Data.CQS.CommandsHandler
{
    public class RemoveArticleCommandHandler:IRequestHandler<RemoveArticleCommand>
    {
        private readonly UserArticleContext _articleContext;

        public RemoveArticleCommandHandler(UserArticleContext articleContext)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));
        }

        public async Task Handle(RemoveArticleCommand request,
            CancellationToken cancellationToken)
        {
            var rt = await _articleContext.Articles.FirstOrDefaultAsync(article => article.Id== request.Id,
                cancellationToken);
            if (rt != null)
            {
                _articleContext.Articles.Remove(rt);
                await _articleContext.SaveChangesAsync(cancellationToken);
                return;
            }
            else
            {
                throw new NullReferenceException("article does not exist");
            }
        }
    }
}
