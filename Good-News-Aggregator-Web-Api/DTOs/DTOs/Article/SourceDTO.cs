using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Article
{
    public class SourceDto
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public Int32 PositiveRate { get; set; }
        public String RssFeedUrl { get; set; }
        public String OriginUrl { get; set; }
    }
}
