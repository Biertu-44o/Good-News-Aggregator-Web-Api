using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Article
{
    public class ShortArticleDto
    {
        public Int32 Id { get; set; }
        public String Title { get; set; }
        public String ArticlePicture { get; set; }
        public Double PositiveRate { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public String ShortDescription { get; set; }
        public String ArticleSourceUrl { get; set; }
        public String SourceName { get; set; }
        public Int32 SourceId { get; set; }
        public String? SortTeg { get; set; }
    }
}
