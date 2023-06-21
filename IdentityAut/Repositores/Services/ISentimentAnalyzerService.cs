using Core.DTOs.Article;

namespace IServices.Services
{
    public interface ISentimentAnalyzerService
    {
        public Task<List<FullArticleDto>> GetArticlesWithSentimentScore(List<FullArticleDto> articles);
    }
}
