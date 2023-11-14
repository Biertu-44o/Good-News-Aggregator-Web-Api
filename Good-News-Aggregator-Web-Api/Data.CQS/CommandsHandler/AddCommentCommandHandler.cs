using Data.CQS.Commands;
using Entities_Context.Entities.UserNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Data.CQS.CommandsHandler
{
    internal class AddCommentCommandHandler : IRequestHandler<AddCommentCommand>
    {
        private readonly UserArticleContext _articleContext;
        private readonly IMapper _mapper;

        public AddCommentCommandHandler(UserArticleContext articleContext, IMapper mapper)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));

            _mapper = mapper ?? throw new NullReferenceException(nameof(mapper));
        }

        public async Task Handle(AddCommentCommand request,
            CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(request.UserEmail))
            {
                throw new ArgumentNullException(nameof(request.UserEmail));
            }

            Int32 userId = await _articleContext.Users.AsNoTracking()
                .Where(x=>x.Email.Equals(request.UserEmail))
                .Select(x=>x.Id)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (userId == 0)
            {
                throw new NullReferenceException(nameof(request.UserEmail));
            }

            if (await _articleContext.Articles.Where(x => x.Id == request.Comment.ArticleId)
                    .AnyAsync(cancellationToken: cancellationToken))
            {
                Comment comment = _mapper.Map<Comment>(request.Comment);
                comment.UserId = userId;

                await _articleContext.Comments.AddAsync(comment, cancellationToken);

                await _articleContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new NullReferenceException(nameof(request));
            }
        }
    }
}
