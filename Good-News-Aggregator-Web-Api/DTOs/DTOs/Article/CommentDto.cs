using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Article
{
    public class CommentDto
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public DateTime DateTime { get; set; }
        public String Text { get; set; }
        public String Name { get; set; }
        public String? ProfilePicture { get; set; }
    }
}
