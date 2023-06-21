using Core.DTOs.Article;

namespace MVC.Models.TegHelperModels
{
    public class ObjectListModel
    {
        public IEnumerable<ShortArticleDto>? ArticlePreviews { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
