using Core.DTOs.Account;
using MediatR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Article;

namespace Data.CQS.Commands
{
    public class UpdateArticleRssCommand : IRequest
    {
        public ConcurrentBag<FullArticleDto> Articles { get; set; }
    }
}
