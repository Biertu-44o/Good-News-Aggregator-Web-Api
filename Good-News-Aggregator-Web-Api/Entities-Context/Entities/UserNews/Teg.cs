using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Core;

namespace Entities_Context.Entities.UserNews
{
    public class Tag 
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }

        public List<ArticleTag> Articles { get; set; }
    }
}
