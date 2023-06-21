using MVC.Filters.Validation.ValidationRules;
using MVC.ValidationRules;

namespace MVC.Models.UserSettings
{
    public class NewUserSettingsViewModel : IValidatePatterns, ValidateErrors
    {

        public String Name { get; set; }

        public String Theme { get; set; }

        public Int32 PositiveRateFilter { get; set; }
    }
}
