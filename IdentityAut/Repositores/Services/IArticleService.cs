using Core.DTOs.Article;

namespace IServices.Services
{
    public interface IArticleService
    {
        public Task<List<ShortArticleDto>?> GetShortArticlesWithSourceByPageAsync(Int32 page, 
            Int32 pageSize, Int32 userRateFilter);
        
        
        public Task<Boolean> DeleteArticleByIdAsync(Int32 id);

        public Task AggregateArticlesDataFromRssSourceAsync(CancellationToken token);

        public Task GetFullContentArticlesAsync();

        public Task RateArticlesAsync();

        public Task<Int32> GetArticleCountAsync( Int32 userRateFilter = 0);
        
        public Task<FullArticleDto?> GetFullArticleByIdAsync(Int32 id);
        
        public Task AggregateArticlesAsync();
    }
}
