using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Data.CQS.Queries
{
    public class GetAllThemesQuery:IRequest<List<String>>
    {
    }
}
