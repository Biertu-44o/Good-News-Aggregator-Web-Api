using Data.CQS.Commands;
using Entities_Context.Entities.UserNews;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Data.CQS.CommandsHandler
{
    public class UpdateUserCommandHandler:IRequestHandler<UpdateUserCommand>
    {
        private readonly UserArticleContext _articleContext;

        public UpdateUserCommandHandler(UserArticleContext articleContext)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (request.Email.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(request.Email));
            }
            if (request.userSettingsDTO==null)
            {
                throw new ArgumentNullException(nameof(request.userSettingsDTO));
            }
            User? User = await _articleContext.Users
                .Where(x => x.Email.Equals(request.Email))
                .Include(x=>x.Theme)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            Int32 ThemeId = await _articleContext.Themes
                .Where(x => x.Theme.Equals(request.userSettingsDTO.Theme))
                .Select(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (User != null && ThemeId != 0)
            {
                if (User.Name != request.userSettingsDTO.Name)
                {
                    User.Name = request.userSettingsDTO.Name;
                }

                if (User.PositiveRateFilter != request.userSettingsDTO.PositiveRateFilter)
                {
                    User.PositiveRateFilter = request.userSettingsDTO.PositiveRateFilter;
                }

                if (User.ThemeId != ThemeId)
                {
                    User.ThemeId = ThemeId;
                }

                await _articleContext.SaveChangesAsync(cancellationToken);
                
            }
            else
            {
                throw new NullReferenceException("User "+request.Email+" does not exist");
            }
        }
    }
}
