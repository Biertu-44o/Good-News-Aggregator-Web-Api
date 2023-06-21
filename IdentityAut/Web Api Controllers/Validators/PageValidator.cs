using FluentValidation;
using Web_Api_Controllers.RequestModels;

namespace Web_Api_Controllers.Validators
{
    public class PageValidator : AbstractValidator<GetArticlesRequest>
    {
        public PageValidator()
        {
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
            RuleFor(x => x.UserFilter).GreaterThanOrEqualTo(0).LessThanOrEqualTo(10);
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        }
    }
}
