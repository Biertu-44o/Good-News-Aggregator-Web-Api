using Data.CQS.Commands;
using Entities_Context.Entities.UserNews;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Data.CQS.CommandsHandler
{
    public class RemoveRefreshTokenCommandHandler : IRequestHandler<RemoveRefreshTokenCommand>
    {
        private readonly UserArticleContext _articleContext;

        public RemoveRefreshTokenCommandHandler(UserArticleContext articleContext)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));
        }

        public async Task Handle(RemoveRefreshTokenCommand request,
            CancellationToken cancellationToken)
        {

            var rt = await _articleContext.RefreshToken
                .AsNoTracking()
                .FirstOrDefaultAsync(token => token.Value.Equals(request.RefreshToken),
                    cancellationToken);

            if (rt != null)
            {
                _articleContext.RefreshToken.Remove(rt);
                await _articleContext.SaveChangesAsync(cancellationToken);
                return;
            }

            throw new NullReferenceException("token does not exist");
        }
    }
}
