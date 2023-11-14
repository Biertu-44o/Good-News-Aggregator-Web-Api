using Core.DTOs.Article;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Web_Api_Controllers.ControllerFactory;
using Web_Api_Controllers.RequestModels;
using Web_Api_Controllers.ResponseModels;

namespace Web_Api_Controllers.Controllers
{
    [ApiController]
    [Route("comments")]
    public class CommentsController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public CommentsController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory ?? throw new NullReferenceException(nameof(serviceFactory));
        }


        /// <summary>
        /// Get all article comments
        /// </summary>
        /// <param name="id">Article id. Greater than 0</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /comments/3034
        ///
        /// </remarks>
        /// <response code="200">Return list of comments</response>
        /// <response code="400">Incorrect id</response>
        [ProducesResponseType(typeof(List<GetCommentsResponse>),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentsByArticleId(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            return Ok((await _serviceFactory.CreateCommentService().GetCommentsByArticleId(id))
                .Select(x => _serviceFactory.CreateMapperService().Map<GetCommentsResponse>(x)));
        }
        /// <summary>
        /// Add new comment.Only authorized users.
        /// </summary>
        /// <param name="comment">Comment model. Article id Greater than 0. Comment text where the length is less than 50 characters</param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /comments
        ///     {
        ///        "id": "3032",
        ///        "text": "nice)",
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">new comment added</response>
        /// <response code="400">Not valid id or comment text</response>
        /// <response code="401">User Unauthorized </response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddNewComment([FromBody] PostCommentRequest comment)
        {
            if (comment.ArticleId < 1 || comment.Text.Length > 50  || String.IsNullOrEmpty(HttpContext.User.Identity!.Name))
            {
                return BadRequest();
            }

            await _serviceFactory.CreateCommentService()
                .AddNewComment(_serviceFactory.CreateMapperService().Map<CommentDto>(comment)
                    , HttpContext.User.Identity!.Name!);
            
            return Ok();
        }

    }
}
