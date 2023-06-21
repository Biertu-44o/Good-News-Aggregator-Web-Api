using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.CQS.Commands
{
    public class RemoveRefreshTokenCommand : IRequest
    {
        public Guid RefreshToken { get; set; }

    }
}
