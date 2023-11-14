using Data.CQS.Queries;
using Entities_Context.Entities.UserNews;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Data.CQS.QueriesHandlers
{
    public class GetUserByEmailQueryHandler:IRequestHandler<GetUserByEmailQuery,User>
    {
        private readonly UserArticleContext _articleContext;


        public GetUserByEmailQueryHandler(UserArticleContext articleContext)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));
        }

        public async Task<User> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(request.Email))
            {
                throw new ArgumentNullException(request.Email);
            }
            
            User? user = await _articleContext.Users
                .AsNoTracking()
                .Where(x => x.Email.Equals(request.Email))
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (user == null)
            {
                throw new NullReferenceException(nameof(request));
            }

            return user;
        }
    }
}
