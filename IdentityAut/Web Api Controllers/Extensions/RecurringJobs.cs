using Hangfire;
using IServices.Services;
using Services.Account;
using Services.Article;
using Web_Api_Controllers.ControllerFactory;

namespace Web_Api_Controllers.Extensions
{

        public static class GoodNewsAggregatorRecurringJobExtension
    {
            public static IServiceCollection GoodNewsAggregatorRecurringJobs
                (this IServiceCollection services)
            {

            var articleService = services.BuildServiceProvider().GetService<IArticleService>();

            RecurringJob.AddOrUpdate("GetArticlesFromRss", () =>
                    articleService.AggregateArticlesDataFromRssSourceAsync(CancellationToken.None),
                "0 0-23 * * *");

            RecurringJob.AddOrUpdate("GetFullContentArticles", () =>
                    articleService.GetFullContentArticlesAsync(),
                "0/50 0-23 * * *");

            RecurringJob.AddOrUpdate("RateArticles", () =>
                    articleService.RateArticlesAsync(),
                "0/55 0-23 * * *");

            return services;
            }
        }
    
}
