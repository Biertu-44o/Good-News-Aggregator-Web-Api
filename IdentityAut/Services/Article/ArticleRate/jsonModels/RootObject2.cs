using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Article.ArticleRate.jsonModels
{
    internal class RootObject2
    {
        public ResponseData responseData { get; set; }
        public bool quotaFinished { get; set; }
        public object mtLangSupported { get; set; }
        public string responseDetails { get; set; }
        public int responseStatus { get; set; }
        public object responderId { get; set; }
        public object exception_code { get; set; }
        public List<Match> matches { get; set; }
    }
}
