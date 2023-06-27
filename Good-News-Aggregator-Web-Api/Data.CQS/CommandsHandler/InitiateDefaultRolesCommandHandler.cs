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
    public class InitiateDefaultRolesCommandHandler:IRequestHandler<InitiateDefaultRolesCommand>
    {
        private readonly UserArticleContext _articleContext;

        public InitiateDefaultRolesCommandHandler(UserArticleContext articleContext)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));
        }

        public async Task Handle(InitiateDefaultRolesCommand request, CancellationToken cancellationToken)
        {
            Boolean anyChanges = false;

            foreach (var role in request.Roles)
            {
                if (!await _articleContext.Roles.AsNoTracking().Where(x => x.Role == role)
                        .AnyAsync(cancellationToken: cancellationToken))
                {
                    await _articleContext.Roles.AddAsync(new UserRole() { Role = role }, cancellationToken);

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
