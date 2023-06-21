using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Article.ArticleRate.jsonModels
{
    public class ResponseData
    {
        public string translatedText { get; set; }
        public double match { get; set; }
    }
}
