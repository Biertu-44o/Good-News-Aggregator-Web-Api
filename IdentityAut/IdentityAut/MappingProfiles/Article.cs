using AutoMapper;
using Core.DTOs.Account;
using Core.DTOs.Article;
using Entities_Context.Entities.UserNews;

namespace MVC.MappingProfiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            SourceMemberNamingConvention = LowerUnderscoreNamingConvention.Instance;
            DestinationMemberNamingConvention = PascalCaseNamingConvention.Instance;

            CreateMap<SourceDto, Source>().ReverseMap();
            CreateMap<FullArticleDto, Article>().ReverseMap();
            CreateMap<ShortArticleDto, Article>().ReverseMap();
            CreateMap<Article, AutoCompleteDataDto>()
                .ForMember(dto => dto.Label,
                    opt
                        => opt.MapFrom(
                            article
                                => article.Title))
                .ForMember(dto => dto.Value, opt => opt.MapFrom(article => article.Id)); //source -> destination
        }
    }

}