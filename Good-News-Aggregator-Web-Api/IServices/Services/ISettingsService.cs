using Core.DTOs.Account;

namespace IServices.Services
{
    public interface ISettingsService
    {

        public Task<userSettingsDTO> GetUserInformationAsync(String email);
        
        public Task<Boolean> UpdateUserSettingsAsync(userSettingsDTO userSettingsDto, String? email);

        public Task<Int32> GetUserArticleRateFilter(String email);
    }
}
