using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Account;
using Entities_Context.Entities.UserNews;
using MediatR;

namespace Data.CQS.Queries
{
    public class GetUserByEmailQuery:IRequest<User>
    {
        public String Email { get; set; }
    }
}
