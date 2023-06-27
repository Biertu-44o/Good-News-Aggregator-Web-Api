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
            

            User? user = await _articleContext.Users
                .Where(x => x.Email.Equals(request.Email))
                .Include(x=>x.Theme)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            Int32 themeId = await _articleContext.Themes
                .AsNoTracking()
                .Where(x => x.Theme.Equals(request.userSettingsDTO.Theme))
                .Select(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (user != null && themeId != 0)
            {
                if (user.Name != request.userSettingsDTO.Name)
                {
                    user.Name = request.userSettingsDTO.Name;
                }

                if (user.PositiveRateFilter != request.userSettingsDTO.PositiveRateFilter)
                {
                    user.PositiveRateFilter = request.userSettingsDTO.PositiveRateFilter;
                }

                if (user.ThemeId != themeId)
                {
                    user.ThemeId = themeId;
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
