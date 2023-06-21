using Data.CQS.Queries;
using Entities_Context.Entities.UserNews;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Data.CQS.QueriesHandlers
{
    public class GetDefaultThemeIdQueryHandler:IRequestHandler<GetDefaultThemeIdQuery,Int32>
    {
        private readonly UserArticleContext _articleContext;

        public GetDefaultThemeIdQueryHandler(UserArticleContext articleContext)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));
        }

        public async Task<Int32> Handle(GetDefaultThemeIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Name.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(request));
            }

            return await _articleContext.Themes
                .Where(x => x.Theme.Equals(request.Name))
                .Select(x=>x.Id)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
