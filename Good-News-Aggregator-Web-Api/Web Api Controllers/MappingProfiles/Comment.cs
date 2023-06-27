using AutoMapper;
using Core.DTOs.Article;
using Entities_Context.Entities.UserNews;
using Web_Api_Controllers.RequestModels;
using Web_Api_Controllers.ResponseModels;

namespace Web_Api_Controllers.MappingProfiles
{
    public class CommentProfile: Profile
    {
        public CommentProfile()
        {
            SourceMemberNamingConvention = LowerUnderscoreNamingConvention.Instance;
            DestinationMemberNamingConvention = PascalCaseNamingConvention.Instance;

            CreateMap<GetCommentsResponse, CommentDto>().ReverseMap();

            CreateMap<PostCommentRequest, CommentDto>().ReverseMap();
            
            CreateMap<CommentDto, Comment>().ReverseMap()
                .ForMember(
                    dest => dest.ArticleId,
                    opt =>
                        opt.MapFrom(src => src.ArticleId)
                )
                .ForMember(
                    dest => dest.Name,
                    opt =>
                        opt.MapFrom(src => src.User.Name)
                )
                .ForMember(
                    dest => dest.ProfilePicture,
                    opt =>
                        opt.MapFrom(src => src.User.ProfilePicture)
                );
        }
    }
}
