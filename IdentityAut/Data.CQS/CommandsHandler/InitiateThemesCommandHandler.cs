using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.CQS.Commands;
using Entities_Context.Entities.UserNews;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Data.CQS.CommandsHandler
{
    public class InitiateThemesCommandHandler : IRequestHandler<InitiateThemesCommand>
    {
        private readonly UserArticleContext _articleContext;

        public InitiateThemesCommandHandler(UserArticleContext articleContext)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));
        }

        public async Task Handle(InitiateThemesCommand request, CancellationToken cancellationToken)
        {
            Boolean anyChanges = false;

            foreach (var theme in request.Themes)
            {
                if (!await _articleContext.Themes
                        .Where(x => x.Theme == theme)
                        .AnyAsync(cancellationToken: cancellationToken))
                {
                    await _articleContext.Themes
                        .AddAsync(new SiteTheme() { Theme = theme }, cancellationToken);

                    anyChanges = true;
                }

            }

            if (anyChanges)
            {
                await _articleContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
