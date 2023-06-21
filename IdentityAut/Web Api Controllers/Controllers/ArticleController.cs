using Core.DTOs.Article;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Web_Api_Controllers.ControllerFactory;
using Web_Api_Controllers.Filters.Errors;
using Web_Api_Controllers.RequestModels;
using Web_Api_Controllers.ResponseModels;

namespace Web_Api_Controllers.Controllers
{

    [ApiController]
    [CustomExceptionFilter]
    [Route("article")]
    public class ArticleController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public ArticleController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory ?? throw new NullReferenceException(nameof(serviceFactory));
        }

        /// <summary>
        /// Get articles by page filtered by rate.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /article?Page=3&amp;PageSize=4&amp;UserFilter=5
        ///
        /// </remarks>
        /// <response code="200">List of articles with a rating higher than the argument</response>
        /// <response code="400">Not valid arguments</response>
        [ProducesResponseType(typeof(IEnumerable<ShortArticleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<IActionResult> GetArticlesByPage([FromQuery] GetArticlesRequest request)
        {
            ValidationResult result = await _serviceFactory
                .CreatePageValidator()
                .ValidateAsync(request);

            if (result.IsValid)
            {
                var articleList = await _serviceFactory
                    .CreateArticlesService()
                    .GetShortArticlesWithSourceByPageAsync(request.Page, request.PageSize, request.UserFilter);

                return Ok(articleList);
            }

            return BadRequest();
        }
        /// <summary>
        /// Get the number of articles with a rating higher than the rate.
        /// </summary>
        /// <param name="rate"> Greater than or equal to 0 and less than or equal to 10.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /article/articles/3
        ///
        /// </remarks>
        /// <response code="200">Articles count with a rating higher than the argument</response>
        /// <response code="400">Not valid arguments</response>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("articles/{rate:int}")]
        public async Task<IActionResult> GetArticlesCount(Int32 rate)
        {
            if (rate >= 0 && rate<=10)
            {
                var articlesCount = await _serviceFactory.CreateArticlesService().GetArticleCountAsync(rate);

                return Ok(articlesCount);
            }

            return BadRequest();
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

        /// <summary>
        /// Aggregate articles from rss. Admin only.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /article
        ///
        /// </remarks>
        /// <response code="200">Aggregation was successful</response>
        /// <response code="401">User Unauthorized</response>
        /// <response code="403">User has no rights</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles="Admin")]
        [HttpPost]
        public async Task<IActionResult> AggregateArticles()
        {
            await _serviceFactory.CreateArticlesService().AggregateArticlesAsync();
            
            return Ok();
        }

    }
}