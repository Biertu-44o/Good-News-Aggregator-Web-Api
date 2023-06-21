using Data.CQS.Queries;
using Entities_Context.Entities.UserNews;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.DTOs.Article;
using Microsoft.EntityFrameworkCore;

namespace Data.CQS.QueriesHandlers
{

        public class GetAllSourcesQueryHandler : IRequestHandler<GetAllSourcesQuery, List<SourceDto>>
        {
            private readonly UserArticleContext _articleContext;
            private readonly IMapper _mapper;
            
            
            public GetAllSourcesQueryHandler(UserArticleContext articleContext, IMapper mapper)
            {
                _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));

            _mapper = mapper ?? throw new NullReferenceException(nameof(mapper));
            }

            public async Task<List<SourceDto>> Handle(GetAllSourcesQuery request, CancellationToken cancellationToken)
            {
                return await _articleContext.Sources.AsNoTracking()
                    .Select(x=>_mapper.Map<SourceDto>(x))
                    .ToListAsync(cancellationToken: cancellationToken);
            }
        }
    
}
