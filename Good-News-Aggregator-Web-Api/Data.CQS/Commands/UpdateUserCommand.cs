using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Account;
using MediatR;

namespace Data.CQS.Commands
{
    public class UpdateUserCommand:IRequest
    {
        public userSettingsDTO userSettingsDTO { get; set; }
        public String Email { get; set; }
    }
}
