using IServices.Repositories;
using IServices;
using Repositories.Implementations;
using Repositories;
using MVC.Filters.Validation;

namespace MVC.Extensions
{
    public static class GoodNewsAggregatorValidationFiltersExtension
    {
        public static IServiceCollection AddGoodNewsAggregatorValidationFilters
            (this IServiceCollection filters)
        {
            filters.AddScoped<SettingsValidationFilterAttribute>();
            filters.AddScoped<LoginValidationFilterAttribute>();
            
            return filters;
        }
    }
}
