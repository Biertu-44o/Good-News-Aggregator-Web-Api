using Core.DTOs.Article;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Web_Api_Controllers.ControllerFactory;
using Web_Api_Controllers.RequestModels;
using Web_Api_Controllers.ResponseModels;

namespace Web_Api_Controllers.Controllers
{

    [ApiController]
    [Route("article")]
    public class ArticleController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public ArticleController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory ?? throw new NullReferenceException(nameof(serviceFactory));
        }

        /// <summary>
        /// Get full article by id.
        /// </summary>
        /// <param name="id">Article id. Greater than 0</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /article/2665
        ///
        /// </remarks>
        /// <response code="200">Article with full content</response>
        /// <response code="400">Invalid id</response>
        /// <response code="404">Article not found</response>
        [ProducesResponseType(typeof(GetArticleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSelectedArticle(Int32 id)
        {
            if (id > 0)
            {
                FullArticleDto? fullArticleDto = await _serviceFactory
                    .CreateArticlesService()
                    .GetFullArticleByIdAsync(id);

                if (fullArticleDto == null)
                {
                    return NotFound();
                }

                return Ok(_serviceFactory.CreateMapperService().Map<GetArticleResponse>(fullArticleDto));
            }

            return BadRequest();
        }

        /// <summary>
        /// Delete article by id. Admin only.
        /// </summary>
        /// <param name="id">Article id. Greater than 0</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /article/2665
        ///
        /// </remarks>
        /// <response code="200">Article was deleted</response>
        /// <response code="400">Invalid id</response>
        /// <response code="401">User Unauthorized</response>
        /// <response code="403">User has no rights</response>
        /// <response code="404">Article not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteArticlesById(Int32 id)
        {
            if (id > 0)
            {
                if (await _serviceFactory.CreateArticlesService().DeleteArticleByIdAsync(id))
                {
                    return Ok();
                }

                return NotFound();
            }
            
            return BadRequest();
        }


    }
}