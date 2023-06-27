using Core.DTOs.Article;

namespace IServices.Services
{
    public interface ISourceService
    {
        public Task<List<SourceDto>> GetAllSourcesAsync();
    }
}
