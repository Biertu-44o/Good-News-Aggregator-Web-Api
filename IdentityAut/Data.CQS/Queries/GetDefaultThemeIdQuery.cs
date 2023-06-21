﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Data.CQS.Queries
{
    public class GetDefaultThemeIdQuery : IRequest<Int32>
    {
        public String Name { get; set; }
    }
}
