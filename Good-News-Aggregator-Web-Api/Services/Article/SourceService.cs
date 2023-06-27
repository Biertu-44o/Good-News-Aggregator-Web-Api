using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.DTOs.Article;
using Data.CQS.Queries;
using Entities_Context;
using Entities_Context.Entities.UserNews;
using IServices;
using IServices.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Services.Article
{

    public class SourceService:ISourceService
    {
        private readonly IMediator _mediator;

        public SourceService(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<List<SourceDto>> GetAllSourcesAsync()
        {
            return await _mediator.Send(new GetAllSourcesQuery());
        }

    }
}
