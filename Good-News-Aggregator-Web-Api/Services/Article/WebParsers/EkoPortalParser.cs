using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Services.Article.WebParsers
{
    internal class EkoPortalParser : AbstractParser
    {
        private readonly HtmlDocument? _htmlDoc;

        public EkoPortalParser(String html)
        {
            HtmlWeb web = new HtmlWeb();
            web.OverrideEncoding = Encoding.UTF8;
            _htmlDoc = web.Load("https://ecoportal.su/news/view/"+html+ ".html");
        }
        
        internal override String GetArticleSourceReference(String id)
        {
            return "https://ecoportal.su/news/view/"+ id + ".html";

        }

        

        internal override String GetPictureReference()
        {
            var imgNode = _htmlDoc.DocumentNode.SelectSingleNode("//newsimage/img");
            var pattern = @"(?<=src="")[^""]+\.jpg(?="")";
            var match = Regex.Match(imgNode.OuterHtml, pattern);
            
            return @"https://ecoportal.su" + match.Value;

        }

        internal override String GetShortDescription()
        {
            var description = _htmlDoc.DocumentNode.SelectSingleNode("//description").InnerHtml;
            return description;
        }

        internal override string GetFullTextDescription()
        {
            var Text = _htmlDoc.DocumentNode.SelectSingleNode("//text");
            
            return Text.InnerHtml;
        }
    }
}
