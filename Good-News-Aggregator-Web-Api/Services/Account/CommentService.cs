using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Article;
using Data.CQS.Commands;
using Data.CQS.Queries;
using IServices.Services;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace Services.Account
{
    public class CommentService : ICommentService
    {
        private readonly IMediator _mediator;
        
        public CommentService(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        public Task<List<CommentDto>> GetCommentsByArticleId(Int32 articleId)
        {
            if (articleId<1)
            {
                throw new ArgumentException(nameof(articleId));
            }

            return _mediator.Send(new GetCommentsByArticleIdQuery()
            {
                articleId = articleId
            });
        }

        public async Task AddNewComment(CommentDto comment, String email)
        {
            if (comment.ArticleId < 1 || comment.Text.IsNullOrEmpty() || comment.Text.Length > 50)
            {
                throw new ArgumentException(nameof(comment));
            }
            if (email.IsNullOrEmpty())
            {
                throw new ArgumentException(nameof(email));
            }
            
            comment.DateTime=DateTime.Now;

            await _mediator.Send(new AddCommentCommand()
            {
                UserEmail = email,
                Comment = comment
            });
        }
    }
}
