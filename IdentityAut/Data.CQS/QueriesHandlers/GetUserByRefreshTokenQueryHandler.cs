using Data.CQS.Queries;
using Entities_Context.Entities.UserNews;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Data.CQS.QueriesHandlers
{

    public class GetUserByRefreshTokenQueryHandler : IRequestHandler<GetUserByRefreshTokenQuery, User>
    {
        private readonly UserArticleContext _articleContext;


        public GetUserByRefreshTokenQueryHandler(UserArticleContext articleContext)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));
        }

        public async Task<User?> Handle(GetUserByRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            if (request.RefreshToken.ToString().IsNullOrEmpty())
            {
                User? user = await _articleContext.RefreshToken.AsNoTracking()
                    .Where(x => x.Value.Equals(request.RefreshToken))
                    .Include(x => x.User)
                    .Select(x => x.User)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                if (user == null)
                {
                    throw new NullReferenceException(nameof(user));
                }
                else
                {
                    return user;
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(request));
            }
        }
    }
}
