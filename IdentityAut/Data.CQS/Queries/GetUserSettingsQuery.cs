using Core.DTOs.Account;
using Entities_Context.Entities.UserNews;
using MediatR;

namespace Data.CQS.Queries
{
    public class GetUserSettingsByEmailQuery : IRequest<userSettingsDTO>
    {
        public String Email { get; set; }
    }
}
