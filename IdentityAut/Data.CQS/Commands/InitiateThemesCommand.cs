using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Data.CQS.Commands
{
    public class InitiateThemesCommand:IRequest
    {
        public String[] Themes { get; set; }
    }
}
