using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.ServiceModel.Syndication;
using System.Xml;
using Core.DTOs.Article;
using Data.CQS.Commands;
using Data.CQS.Queries;
using IServices.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Moq;
using Services.Article;
using Services.Article.ArticleRate;

namespace Tests
{
    public class ArticleServiceTests
    {
        private readonly Mock<IMediator> _mediatorMock = new Mock<IMediator>();
        private readonly Mock<ISourceService> _sourceServiceMock = new Mock<ISourceService>();
        private readonly Mock<IConfiguration> _ñonfigurationMock = new Mock<IConfiguration>();
        private readonly Mock<ISentimentAnalyzerService> _syndicationFeedMock = new Mock<ISentimentAnalyzerService>();


        private ArticleService CreateService()
        {
            var articleService = new ArticleService(
                _sourceServiceMock.Object,
                _ñonfigurationMock.Object,
                _mediatorMock.Object,
                _syndicationFeedMock.Object
            );

            return articleService;
        }

        [Theory]
        [InlineData(0, 2, 3)]
        [InlineData(-1, 10, 4)]
        [InlineData(3, -12, 7)]
        [InlineData(4, 0, 8)]
        [InlineData(5, 3, 11)]
        [InlineData(3, 2, -3)]
        public async void
            GetShortArticlesWithSourceByPageAsync_WithIncorrectPageAndPageSizeAndRateFilter_ReturnCorrectError
            (Int32 page, Int32 pageSize, Int32 userRateFilter)
        {

            _mediatorMock.Setup(mediator => mediator.Send(
                    It.IsAny<GetArticleByPageQuery>(), CancellationToken.None))
                .ReturnsAsync(new List<ShortArticleDto> { new ShortArticleDto(), new ShortArticleDto() });

            var articleService = CreateService();

            var result = async () => await articleService
                .GetShortArticlesWithSourceByPageAsync(page, pageSize, userRateFilter);

            await Assert.ThrowsAnyAsync<ArgumentException>(result);
        }

        [Fact]
        public async void DeleteArticleByIdAsync_WithNonExistentId_ReturnFalse()
        {

            _mediatorMock.Setup(mediator => mediator.Send(
                    It.IsAny<RemoveArticleCommand>(), CancellationToken.None))
                .Throws(new NullReferenceException());

            var articleService = CreateService();

            var result = await articleService
                .DeleteArticleByIdAsync(1000);

            Assert.False(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async void DeleteArticleByIdAsync_WithInCorrectId_ReturnCorrectError(Int32 id)
        {

            _mediatorMock.Setup(mediator => mediator.Send(
                    It.IsAny<RemoveArticleCommand>(), CancellationToken.None));

            var articleService = CreateService();

            var result = async () => await articleService
                .DeleteArticleByIdAsync(id);

            await Assert.ThrowsAnyAsync<ArgumentException>(result);
        }

        [Fact]
        public async void DeleteArticleByIdAsync_WithCorrectId_ReturnTrue()
        {

            _mediatorMock.Setup(mediator => mediator.Send(
                    It.IsAny<RemoveArticleCommand>(), CancellationToken.None));


            var articleService = CreateService();

            var result = await articleService
                .DeleteArticleByIdAsync(1000);

            Assert.True(result);
        }

        [Fact]
        public async void GetFullArticleByIdAsync_WithNonExistentId_ReturnFalse()
        {

            _mediatorMock.Setup(mediator => mediator.Send(
                    It.IsAny<GetFullArticleByIdQuery>(), CancellationToken.None))
                .ReturnsAsync((FullArticleDto?)null);

            var articleService = CreateService();

            var result = await articleService
                .GetFullArticleByIdAsync(1000);

            Assert.Null(result);
        }

        [Fact]
        public async void GetFullArticleByIdAsync_WithCorrectId_ReturnTrue()
        {

            _mediatorMock.Setup(mediator => mediator.Send(
                It.IsAny<GetFullArticleByIdQuery>(), CancellationToken.None))
                .ReturnsAsync(new FullArticleDto()
                {
                    SourceId = 12
                });

            var articleService = CreateService();

            var result = await articleService
                .GetFullArticleByIdAsync(1000);

            Assert.IsType<FullArticleDto>(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async void GetFullArticleByIdAsync_WithInCorrectId_ReturnCorrectError(Int32 id)
        {

            _mediatorMock.Setup(mediator => mediator.Send(
                It.IsAny<GetFullArticleByIdQuery>(), CancellationToken.None));

            var articleService = CreateService();

            var result = async () => await articleService
                .GetFullArticleByIdAsync(id);

            await Assert.ThrowsAnyAsync<ArgumentException>(result);
        }



        [Fact]
        public async void GetFullContentArticlesAsync_WithCorrectOnlinerArticles_CorrectAddToDb()
        {

            _mediatorMock.Setup(mediator => mediator.Send(
                    It.IsAny<GetArticlesWithoutContentQuery>(), CancellationToken.None))
                .ReturnsAsync(new List<FullArticleDto>
                {
                    new FullArticleDto()
                    {
                        ArticleSourceUrl = "https://auto.onliner.by/2023/06/09/prokatila-na-kapote-byvshego",
                        SourceName = "Onliner"
                    },
                    new FullArticleDto()
                    {
                        ArticleSourceUrl = "",
                        SourceName = "Onliner"
                    }
                });

            _ñonfigurationMock.Setup(configuration => configuration[It.IsAny<String>()])
                .Returns("Services.Article.WebParsers.OnlinerParser");


            List<UpdateArticleRssCommand> savedCommands = new List<UpdateArticleRssCommand>();
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<UpdateArticleRssCommand>(), CancellationToken.None))
                .Callback<UpdateArticleRssCommand, CancellationToken>((command, cancellationToken) =>
                {
                    savedCommands.Add(command);
                })
                .Returns(Task.CompletedTask);


            var articleService = CreateService();

            await articleService
                .GetFullContentArticlesAsync();

            Assert.Single(savedCommands);
            
            Assert.NotNull(savedCommands[0].Articles.FirstOrDefault()?.ArticlePicture);
            
            Assert.NotNull(savedCommands[0].Articles.FirstOrDefault()?.ShortDescription);

            Assert.NotNull(savedCommands[0].Articles.FirstOrDefault()?.FullText);

            Assert.NotNull(savedCommands[0].Articles.FirstOrDefault()?.ArticleSourceUrl);

        }


        [Fact]
        public async void GetFullContentArticlesAsync_WithCorrectECOportalArticles_CorrectAddToDb()
        {

            _mediatorMock.Setup(mediator => mediator.Send(
                    It.IsAny<GetArticlesWithoutContentQuery>(), CancellationToken.None))
                .ReturnsAsync(new List<FullArticleDto>
                {
                    new FullArticleDto()
                    {
                        ArticleSourceUrl = "120662",
                        SourceName = "ECOportal"
                    },
                    new FullArticleDto()
                    {
                        ArticleSourceUrl = "",
                        SourceName = "ECOportal"
                    }

                });

            _ñonfigurationMock.Setup(configuration => configuration[It.IsAny<String>()])
                .Returns("Services.Article.WebParsers.EkoPortalParser");


            List<UpdateArticleRssCommand> savedCommands = new List<UpdateArticleRssCommand>();
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<UpdateArticleRssCommand>(), CancellationToken.None))
                .Callback<UpdateArticleRssCommand, CancellationToken>((command, cancellationToken) =>
                {
                    savedCommands.Add(command);
                })
                .Returns(Task.CompletedTask);


            var articleService = CreateService();

            await articleService
                .GetFullContentArticlesAsync();

            Assert.Single(savedCommands);

            Assert.NotNull(savedCommands[0].Articles.FirstOrDefault()?.ArticlePicture);

            Assert.NotNull(savedCommands[0].Articles.FirstOrDefault()?.ShortDescription);

            Assert.NotNull(savedCommands[0].Articles.FirstOrDefault()?.FullText);

            Assert.NotNull(savedCommands[0].Articles.FirstOrDefault()?.ArticleSourceUrl);

        }

        [Fact]
        public async void GetFullContentArticlesAsync_WithInCorrectArticleSource_CorrectAddToDb()
        {

            _mediatorMock.Setup(mediator => mediator.Send(
                    It.IsAny<GetArticlesWithoutContentQuery>(), CancellationToken.None))
                .ReturnsAsync(new List<FullArticleDto>
                {
                    new FullArticleDto()
                    {
                        ArticleSourceUrl = "120715",
                        SourceName = "Unknown source"
                    }

                });

            _ñonfigurationMock.Setup(configuration => configuration[It.IsAny<String>()])
                .Returns("");


            List<UpdateArticleRssCommand> savedCommands = new List<UpdateArticleRssCommand>();
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<UpdateArticleRssCommand>(), CancellationToken.None))
                .Callback<UpdateArticleRssCommand, CancellationToken>((command, cancellationToken) =>
                {
                    savedCommands.Add(command);
                })
                .Returns(Task.CompletedTask);


            var articleService = CreateService();

            await Assert.ThrowsAnyAsync<ArgumentException>(async () =>
            {
                await articleService.GetFullContentArticlesAsync();
            });
        }

        [Fact]
        public async void RateArticlesAsync_WithEmptyArticleList_CorrectAddToDb()
        {

            _mediatorMock.Setup(mediator => mediator.Send(
                    It.IsAny<GetArticlesWithoutRateQuery>(), CancellationToken.None))
                .ReturnsAsync(new List<FullArticleDto>());


            List<RateArticlesCommand> savedCommands = new List<RateArticlesCommand>();
            
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<RateArticlesCommand>(), CancellationToken.None))
                .Callback<RateArticlesCommand, CancellationToken>((command, cancellationToken) =>
                {
                    savedCommands.Add(command);
                })
                .Returns(Task.CompletedTask);

            var articleService = CreateService();

            await articleService.RateArticlesAsync();

            Assert.Empty(savedCommands);
        }
    }
}