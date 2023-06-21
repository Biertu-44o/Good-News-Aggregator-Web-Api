using Entities_Context.Entities.UserNews;
using IServices.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Implementations;

public class ArticleRepository : Repository<Article>, IArticleRepository
{
    public ArticleRepository(UserArticleContext newsAggregatorContext)
        : base(newsAggregatorContext)
    {
    }

    public async Task<List<Article>> GetArticlesByTagByPageAsync(Int32 page, Int32 pageSize, Int32 tagId, Int32 userRateFilter)
    {
        
        var articles = await DbSet
            .Include(article => article.Source)
            .Include(x=>x.Tags)
            .Where(article => article.Tags.Any(tag => tag.TagId == tagId)&& article.PositiveRate >= userRateFilter)
            .OrderByDescending(x => x.DateTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .OrderByDescending(x => x.DateTime)
            .ToListAsync();

        return articles;
    }
    public async Task<List<Article>> GetArticlesByPageAsync(Int32 page, Int32 pageSize, Int32 userRateFilter)
    {
        var articles = await DbSet
            .Where(article=>article.PositiveRate>=userRateFilter)
            .OrderByDescending(x => x.DateTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return articles;
    }

    public async Task<List<Article>> GetArticlesBySearchRequestByPageAsync(Int32 page, Int32 pageSize, String searchLineRequest)
    {
        var articles = await DbSet
            .Include(article => article.Source)
            .Where(article => article.Title.Contains(searchLineRequest))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .OrderByDescending(x => x.DateTime)
            .ToListAsync();

        return articles;
    }
}