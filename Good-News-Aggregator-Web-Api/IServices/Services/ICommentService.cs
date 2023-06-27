using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Article;

namespace IServices.Services
{
    public interface ICommentService
    {
        public Task<List<CommentDto>> GetCommentsByArticleId(Int32 articleId);

        public Task AddNewComment(CommentDto comment, String email);
    }
}
