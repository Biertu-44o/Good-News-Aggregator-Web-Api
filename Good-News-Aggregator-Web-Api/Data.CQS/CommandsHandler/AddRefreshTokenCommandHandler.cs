using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.CQS.Commands;
using Entities_Context.Entities.UserNews;
using MediatR;

namespace Data.CQS.CommandsHandler
{
    public class AddRefreshTokenCommandHandler : IRequestHandler<AddRefreshTokenCommand>
    {
        private readonly UserArticleContext _articleContext;

        public AddRefreshTokenCommandHandler(UserArticleContext articleContext)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));
        }

        public async Task Handle(AddRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            await _articleContext.RefreshToken.AddAsync(new RefreshToken()
            {
                UserId = request.Id,
                Value = request.Value
            }, cancellationToken);

            await _articleContext.SaveChangesAsync(cancellationToken);

        }
    }
}
