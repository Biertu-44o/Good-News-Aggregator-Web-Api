using Core;

namespace Entities_Context.Entities.UserNews
{
    public class Article
    {
        public Int32 Id { get; set; }
        public String HashUrlId { get; set; }
        public String Title { get; set; }
        public String? ShortDescription { get; set; }
        public String? FullText { get; set; }
        public Double PositiveRate { get; set; }
        public Double FirstRate { get; set; }
        public Double SecondRate { get; set; }
        public String ArticleSourceUrl { get; set; }
        public String? ArticlePicture { get; set; }
        public DateTime DateTime { get; set; }
        public Int32 SourceId { get; set; }
        public Source Source { get; set; }

        public List<ArticleTag> Tags{ get; set; }

        public List<Comment> Comments { get; set; }
    }
}