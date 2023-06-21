using Data.CQS.Queries;
using Entities_Context.Entities.UserNews;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Data.CQS.QueriesHandlers
{
    public class GetUserRolesByUserIdQueryHandler:IRequestHandler<GetUserRolesByUserIdQuery,List<UserRole>>
    {
        private readonly UserArticleContext _articleContext;

        public GetUserRolesByUserIdQueryHandler(UserArticleContext articleContext)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));
        }
        public async Task<List<UserRole>> Handle(GetUserRolesByUserIdQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == 0)
            {
                throw new ArgumentNullException(nameof(request));
            }
            return await _articleContext.UsersRoles
                .Where(x => x.UserId == request.UserId)
                .Include(x => x.Role)
                .Select(x=>x.Role)
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
