using Entities_Context.Entities.UserNews;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.CQS.Queries
{

    public class GetUserByRefreshTokenQuery : IRequest<User>
    {
        public Guid RefreshToken { get; set; }
    }
}
