using AutoMapper;
using Core.DTOs.Article;
using Data.CQS.Commands;
using Entities_Context.Entities.UserNews;
using Web_Api_Controllers.ResponseModels;

namespace Web_Api_Controllers.MappingProfiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            SourceMemberNamingConvention = LowerUnderscoreNamingConvention.Instance;
            DestinationMemberNamingConvention = PascalCaseNamingConvention.Instance;
            
            CreateMap<AddFullArticlesCommand, Article>().ReverseMap();
            CreateMap<AddFullArticlesCommand, FullArticleDto>().ReverseMap();

            CreateMap<SourceDto, Source>().ReverseMap();
            CreateMap<FullArticleDto, GetArticleResponse>().ReverseMap();
            CreateMap<FullArticleDto, Article>().ReverseMap()
                .ForMember(
                dest => dest.SourceName,
                opt =>
                    opt.MapFrom(src => src.Source.Name)
                    )
                .ForMember(
                dest => dest.SourceUrl,
                opt =>
                    opt.MapFrom(src => src.Source.OriginUrl)
            );
            CreateMap<ShortArticleDto, Article>().ReverseMap()
                .ForMember(
                    dest => dest.SourceName,
                    opt=>
                        opt.MapFrom(src=>src.Source.Name)
                ); 
        }
    }

}