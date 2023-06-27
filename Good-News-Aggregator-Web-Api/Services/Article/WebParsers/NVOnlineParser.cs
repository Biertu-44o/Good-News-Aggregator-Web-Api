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
    internal class NVOnlineParser : AbstractParser
    {
        private readonly HtmlDocument? _htmlDoc;

        public NVOnlineParser(String html)
        {
            HtmlWeb web = new HtmlWeb();

            _htmlDoc = web.Load(html);
        }

        internal override String GetArticleSourceReference(String id)
        {
            return id;
        }

        internal override String GetPictureReference()
        {
            var picture = _htmlDoc.DocumentNode.SelectSingleNode("//img[@class = 'entry-thumb']").Attributes["src"].Value;

            return picture;

        }

        internal override String GetShortDescription()
        {
            String shortDescription = _htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'td-post-content tagdiv-type']/p").InnerHtml;

            String plainText = Regex.Replace(shortDescription, "<.*?>", string.Empty);

            plainText = Regex.Replace(plainText, @"<.*?>|https?://\S+|www\.\S+", string.Empty);

            if (plainText.Length > 300)
            {
                plainText = plainText.Substring(0, 300) + "...";
            }

            return shortDescription;
        }

        internal override string GetFullTextDescription()
        {
            var text = _htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'td-post-content tagdiv-type']");

            var nodesToRemove = text.SelectNodes(@"//div[@class = 'td-a-rec td-a-rec-id-content_inlineleft  tdi_2 td_block_template_1']|
		//div[@class = 'td-a-rec td-a-rec-id-content_bottom  tdi_3 td_block_template_1']");

            if (nodesToRemove !=null)
            {
                foreach (var nodes in nodesToRemove)
                {
                    nodes.Remove();
                }
            }

            var resultText = Regex
                .Replace(text.InnerHtml, @"<script\b[^>]*>(.*?)</script>", "",
                    RegexOptions.Singleline | RegexOptions.IgnoreCase);

            return resultText;
        }
    }
}
