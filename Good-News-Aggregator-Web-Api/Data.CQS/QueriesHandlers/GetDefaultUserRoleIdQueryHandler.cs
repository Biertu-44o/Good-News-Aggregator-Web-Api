using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.CQS.Queries;
using Entities_Context.Entities.UserNews;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Data.CQS.QueriesHandlers
{
    public class GetDefaultUserRoleIdQueryHandler : IRequestHandler<GetDefaultUserRoleQuery,UserRole?>
    {
        private readonly UserArticleContext _articleContext;

        public GetDefaultUserRoleIdQueryHandler(UserArticleContext articleContext)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));
        }

        public async Task<UserRole> Handle(GetDefaultUserRoleQuery request, CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(request.RoleName))
            {
                throw new ArgumentNullException(nameof(request));
            }

            UserRole? role = await _articleContext.Roles
                .Where(x => x.Role.Equals(request.RoleName))
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);


            return role;
        }
    }
}
