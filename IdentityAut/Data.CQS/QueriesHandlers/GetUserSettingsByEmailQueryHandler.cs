using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.DTOs.Account;
using Data.CQS.Queries;
using Entities_Context.Entities.UserNews;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Data.CQS.QueriesHandlers
{
    public class GetUserSettingsByEmailQueryHandler:IRequestHandler<GetUserSettingsByEmailQuery, userSettingsDTO>
    {
        private readonly UserArticleContext _articleContext;
        private readonly IMapper _mapper;

        public GetUserSettingsByEmailQueryHandler(UserArticleContext articleContext, IMapper mapper)
        {
            _articleContext = articleContext ?? throw new NullReferenceException(nameof(articleContext));
            _mapper = mapper ?? throw new NullReferenceException(nameof(mapper));
        }

        public async Task<userSettingsDTO> Handle(GetUserSettingsByEmailQuery request, CancellationToken cancellationToken)
        {
            User? user = await _articleContext.Users
                .Where(x => x.Email.Equals(request.Email))
                .Include(x=>x.Theme)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (user != null)
            {
                userSettingsDTO userDto = _mapper.Map<userSettingsDTO>(user);
                
                userDto.ProfilePictureByteArray = new PictureBase64EncoderDecoder().PictureDecoder(user.ProfilePicture);
                
                return userDto;
                
            }

            throw new ArgumentNullException("User " + request.Email + " not found");
        }
    }

    internal class PictureBase64EncoderDecoder
    {
        internal Byte[]? PictureDecoder(String byteString)
        {
            try
            {
                return Convert.FromBase64String(byteString);
            }
            catch
            {
                return null;
            }
        }
    }
}
