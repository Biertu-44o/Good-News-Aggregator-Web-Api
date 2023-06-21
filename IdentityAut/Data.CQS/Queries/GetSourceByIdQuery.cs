using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities_Context.Entities.UserNews;
using MediatR;

namespace Data.CQS.Queries
{
    public class GetArticlesHashIdBySourceIdQuery : IRequest<List<String>>
    {
        public Int32 Id { get; set; }
    }
}
