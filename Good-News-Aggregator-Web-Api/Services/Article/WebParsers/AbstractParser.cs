using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Article.WebParsers
{
    internal abstract class AbstractParser
    {

        internal abstract String GetPictureReference();
        internal abstract String GetShortDescription();
        internal abstract String GetFullTextDescription();
        internal abstract String GetArticleSourceReference(String id);
    }
}
