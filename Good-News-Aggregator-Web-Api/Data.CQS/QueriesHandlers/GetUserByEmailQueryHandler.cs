using System.Formats.Asn1;
using AutoMapper;
using Core.DTOs.Account;
using Data.CQS.Queries;
using Entities_Context.Entities.UserNews;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
            if (request.Email.IsNullOrEmpty())
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
