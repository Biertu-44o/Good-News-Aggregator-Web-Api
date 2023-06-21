namespace IServices.Services
{
    public interface IUiThemeService
    {
        public Task InitiateThemeAsync();

        public Task<Int32> GetIdDefaultThemeAsync();

        public Task<List<String>> GetAllThemesAsync();
    }
}
