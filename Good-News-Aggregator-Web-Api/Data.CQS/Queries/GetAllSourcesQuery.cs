using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Article;
using MediatR;

namespace Data.CQS.Queries
{
    public class GetAllSourcesQuery:IRequest<List<SourceDto>>
    {
    }
}
