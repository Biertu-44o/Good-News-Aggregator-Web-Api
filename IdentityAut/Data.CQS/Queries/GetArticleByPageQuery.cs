using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Article;
using MediatR;

namespace Data.CQS.Queries
{
    public class GetArticleByPageQuery : IRequest<List<ShortArticleDto>>
    {
        public Int32 Page { get; set; }
        public Int32 Count { get; set; }
        public Int32 UserFilter { get; set; }

    }
}
