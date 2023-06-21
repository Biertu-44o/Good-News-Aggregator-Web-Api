using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Data.CQS.Commands;
using Entities_Context.Entities.UserNews;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Data.CQS.CommandsHandler
{
    public class CreateUserWithRoleCommandHandler:IRequestHandler<CreateUserWithRoleCommand>
    {
        private readonly UserArticleContext _articleContext;
        private readonly IMapper _mapper;

        public CreateUserWithRoleCommandHandler(UserArticleContext articleContext, IMapper mapper)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));

            _mapper = mapper ?? throw new NullReferenceException(nameof(mapper));
        }

        public async Task Handle(CreateUserWithRoleCommand request,
            CancellationToken cancellationToken)
        {

            if (!await _articleContext.Roles
                    .Where(x=>x.Id==request.Role.Id)
                    .AnyAsync(cancellationToken: cancellationToken))
            {
                throw new ArgumentNullException(nameof(request.Role));
            }

            await _articleContext.Users.AddAsync(_mapper.Map<User>(request.User), cancellationToken);

            await _articleContext.SaveChangesAsync(cancellationToken);

            UsersRoles newUserRole = new UsersRoles()
            {
                RoleId = request.Role.Id,

                UserId = ((await _articleContext.Users
                    .Where(x => x.Email.Equals(request.User.Email))
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken))!).Id
            };

            await _articleContext.UsersRoles.AddAsync(newUserRole, cancellationToken);

            await _articleContext.SaveChangesAsync(cancellationToken);
        }
    }
}
