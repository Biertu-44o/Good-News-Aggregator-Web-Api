using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Article.ArticleRate.jsonModels
{
    internal class Probability
    {
        public double neg { get; set; }
        public double neutral { get; set; }
        public double pos { get; set; }
    }
}
