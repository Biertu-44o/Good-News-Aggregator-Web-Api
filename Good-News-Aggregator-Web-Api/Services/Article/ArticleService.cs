using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using AutoMapper;
using Core.DTOs.Article;
using IServices;
using Microsoft.EntityFrameworkCore;
using System.ServiceModel.Syndication;
using Data.CQS.Queries;
using IServices.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Services.Article.ArticleRate;
using Services.Article.WebParsers;
using Serilog;
using Data.CQS.QueriesHandlers;
using System.Drawing.Printing;
using Data.CQS.Commands;

namespace Services.Article
{
    public class ArticleService : IArticleService
    {
        private readonly IMediator _mediator;
        private readonly ISourceService _sourceService;
        private readonly IConfiguration _сonfiguration;
        private readonly ISentimentAnalyzerService _sentimentAnalyzerService;


        public ArticleService(
            ISourceService sourceService,
            IConfiguration сonfiguration,
            IMediator mediator,
            ISentimentAnalyzerService sentimentAnalyzerService)
        {
            _sourceService = sourceService ?? throw new ArgumentNullException(nameof(sourceService));

            _сonfiguration = сonfiguration ?? throw new ArgumentNullException(nameof(сonfiguration));

            _mediator = mediator ?? throw new ArgumentNullException(nameof(сonfiguration));

            _sentimentAnalyzerService = sentimentAnalyzerService ?? throw new ArgumentNullException(nameof(sentimentAnalyzerService));
        }


        public async Task<List<ShortArticleDto>?> GetShortArticlesWithSourceByPageAsync(Int32 page, Int32 pageSize, Int32 userRateFilter)
        {
            if (page<=0 || pageSize<=0 || userRateFilter<0 || userRateFilter>10)
            {
                Log.Error("attempt to use incorrect pagination arguments:" +
                            "page={0},pageSize={1},userFilter={2}",page,pageSize,userRateFilter);
                
                throw new ArgumentException();
            }

            List<ShortArticleDto>? articleList = await _mediator.Send(new GetArticleByPageQuery()
            {
                Page = page,
                Count = pageSize,
                UserFilter = userRateFilter

            });

            return articleList;
        }

        public async Task<Boolean> DeleteArticleByIdAsync(Int32 id)
        {
            if (id<1)
            {
                throw new ArgumentException("attempt to delete article with incorrect id: "+id);
            }
            
            try
            {
                await _mediator.Send(new RemoveArticleCommand()
                {
                    Id = id
                });

                return true;
            }
            catch (NullReferenceException)
            {
                Log.Warning("Article to delete {0} is not found", id);

                return false;
            }
        }

        public async Task<Int32> GetArticleCountAsync(Int32 userRateFilter)
        {
            return await _mediator.Send(new GetArticleCountQuery()
            {
                UserRateFilter = userRateFilter
            });
        }

        public async Task<FullArticleDto?> GetFullArticleByIdAsync(Int32 id)
        {

            if (id < 1)
            {
                throw new ArgumentException("attempt to delete article with incorrect id: " + id);
            }


            FullArticleDto? fullArticle = await _mediator.Send(new GetFullArticleByIdQuery()
            {
                Id = id

            });

            if (fullArticle == null)
            {
                Log.Warning("Article {id} is not found", id);
                return null;
            }

            return fullArticle;
        }


        public async Task AggregateArticlesAsync()
        {

            await AggregateArticlesDataFromRssSourceAsync(CancellationToken.None);

            await GetFullContentArticlesAsync();

            await RateArticlesAsync();

        }

        public async Task RateArticlesAsync()
        {

            Log.Information("evaluation of articles has been started");

            var articleWithoutRate = await _mediator.Send(new GetArticlesWithoutRateQuery());

            if (articleWithoutRate.Count == 0)
            {
                Log.Information("All articles has sentiment score");
                return;
            }

            var articlesToUpdate = await _sentimentAnalyzerService.GetArticlesWithSentimentScore(articleWithoutRate);

            await _mediator.Send(new RateArticlesCommand()
            {
                Articles = articlesToUpdate
            });


            Log.Information("the evaluation of the news has ended.{0} articles get sentiment score", 
                articlesToUpdate.Count);
        }


        public async Task GetFullContentArticlesAsync()
        {
            Log.Information("News parse has been started");

            var concBag = new ConcurrentBag<FullArticleDto>();

            var articleWithoutContent = await _mediator.Send(new GetArticlesWithoutContentQuery());

            if (articleWithoutContent.Count == 0)
            {
                Log.Information("All news has content");
                return;
            }

            await Parallel.ForEachAsync(articleWithoutContent, (dto, token) =>
            {
                var sourceConfigValue = _сonfiguration["ResourceHandlers:" + dto.SourceName];

                if (!String.IsNullOrEmpty(sourceConfigValue))
                {
                    try
                    {
                        var parser =
                            Activator.CreateInstance(Type.GetType(sourceConfigValue)
                                                     ?? throw new InvalidOperationException(), dto.ArticleSourceUrl) as
                                AbstractParser;

                        dto.ArticlePicture = parser.GetPictureReference();
                        dto.ShortDescription = parser.GetShortDescription();
                        dto.FullText = parser.GetFullTextDescription();
                        dto.ArticleSourceUrl = parser.GetArticleSourceReference(dto.ArticleSourceUrl);

                    }
                    catch (Exception e)
                    {
                        Log.Warning("Unsuccessful attempt to aggregate news: {0} , {1}",
                            dto.ArticleSourceUrl, e.Message);
                        return ValueTask.CompletedTask;
                    }


                    concBag.Add(dto);
                }
                else
                {
                    throw new ArgumentException("Source was not found in the configuration file "+dto.SourceUrl);
                }

                return ValueTask.CompletedTask;
            });

            await _mediator.Send(new UpdateArticleRssCommand()
            {
                Articles = concBag
            });

            Log.Information("News parse has been ended.{0} articles updated its content",concBag.Count);
        }

        private async Task AddArticlesAsync(IEnumerable<FullArticleDto> articleDto)
        {
            await _mediator.Send(new AddFullArticlesCommand()
            {
                Articles = articleDto
            });
        }


        public async Task AggregateArticlesDataFromRssSourceAsync(CancellationToken cancellationToken)
        {

            Log.Information("articles rss aggregate has been started");

            List<SourceDto>? sources = await _sourceService.GetAllSourcesAsync();

            var articles = new ConcurrentBag<FullArticleDto>();

            List<Task> tasks = new List<Task>();

            foreach (var source in sources)
            {
                var articleHash = await GetArticlesHashIdBySourceIdAsync(source.Id);

                tasks.Add(Task.Run(async () =>
                {
                    using (var reader = XmlReader.Create(source.RssFeedUrl))
                    {
                        var feed = SyndicationFeed.Load(reader);

                        var articleTasks = feed.Items
                            .Where(item => !articleHash.Contains(ComputeSha256Hash(source.RssFeedUrl + item.Id)))
                            .Select(async item =>
                            {
                                FullArticleDto articleDto = new FullArticleDto()
                                {
                                    ArticleSourceUrl = item.Id,
                                    HashUrlId = ComputeSha256Hash(source.RssFeedUrl + item.Id),
                                    SourceId = source.Id,
                                    SourceUrl = source.OriginUrl,
                                    SourceName = source.Name,
                                    ArticleTags = new List<String>(),
                                    Title = item.Title.Text,
                                    DateTime = item.PublishDate.DateTime
                                };

                                articles.Add(articleDto);
                                await Task.CompletedTask;
                            });

                        await Task.WhenAll(articleTasks);
                        
                        reader.Close();
                    }
                }));
            }

            await Task.WhenAll(tasks);

            await AddArticlesAsync(articles.ToList());

            Log.Information("articles rss aggregate has been ended.{0} articles received from rss",articles.Count);
        }

        public string ComputeSha256Hash(String input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private async Task<List<String>?> GetArticlesHashIdBySourceIdAsync(Int32 sourceId)
        {
            return await _mediator.Send(new GetArticlesHashIdBySourceIdQuery()
            {
                Id = sourceId
            });

        }
    }
}
