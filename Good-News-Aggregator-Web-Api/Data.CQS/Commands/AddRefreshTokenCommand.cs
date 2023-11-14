using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Data.CQS.Commands
{
    public class AddRefreshTokenCommand:IRequest
    {
        public Guid Value { get; set; }
        public Int32 Id { get; set; }
    }
}
