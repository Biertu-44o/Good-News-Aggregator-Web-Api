﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Article;
using MediatR;

namespace Data.CQS.Commands
{
    public class AddCommentCommand: IRequest
    {
        public CommentDto Comment { get; set; }
        public String UserEmail { get; set; }
    }
}
