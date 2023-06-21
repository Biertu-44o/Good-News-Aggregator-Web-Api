using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MVC.Filters.Validation.ValidationRules;
using MVC.ValidationRules;

namespace MVC.Models.UserSettings
{
    public class ShowUserInfoAndConfigViewModel: IValidatePatterns, ValidateErrors
    {
        [Required(ErrorMessage = ValidateErrors.NoName)]
        [RegularExpression(IValidatePatterns.NamePattern
            , ErrorMessage = ValidateErrors.BadName)]
        public String Name { get; set; }
        
        [Required]
        [Remote("isThemeExist", "Settings",
            ErrorMessage = ValidateErrors.NoTheme)]
        public String Theme { get; set; }

        public List<String> AllThemes { get; set; }
        
        public Int32 PositiveRate { get; set; }
        
        [Required(ErrorMessage = ValidateErrors.NoRateFilter)]
        [Range(1, 10, ErrorMessage = ValidateErrors.BadRateFilter)]
        public Int32 PositiveRateFilter { get; set; }

        public byte[]? ProfilePicture { get; set; }
    }
}
