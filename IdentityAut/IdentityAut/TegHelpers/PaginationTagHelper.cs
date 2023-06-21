using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MVC.Models.TegHelperModels;

namespace MVC.TegHelpers
{
    public class PaginationTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public PaginationTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        public PageInfo PageInfo { get; set; }
        public string PageAction { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }


        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            var result = new TagBuilder("div");

            for (int i = 1; i <= PageInfo.TotalPages; i++)
            {
                var tag = new TagBuilder("a");
                

                tag.AddCssClass("custom-button-2 custom-button-3");
                if (ViewContext.HttpContext.Request.Query.ContainsKey("page") && int.TryParse(
                        ViewContext.HttpContext.Request.Query["page"],
                        out var actualPage) && i>= actualPage-2 && i <= actualPage + 2)
                {
                    if (i == actualPage)
                    {
                        tag.AddCssClass("active-btn");
                    }

                    var anchorInnerHtml = i.ToString();
                    tag.Attributes["href"] = urlHelper.Action(PageAction, new { page = i });
                    tag.InnerHtml.Append(anchorInnerHtml);
                    result.InnerHtml.AppendHtml(tag);
                }

                if (!ViewContext.HttpContext.Request.Query.ContainsKey("page") && i<=3)
                {
                    var anchorInnerHtml = i.ToString();
                    tag.Attributes["href"] = urlHelper.Action(PageAction, new { page = i });
                    tag.InnerHtml.Append(anchorInnerHtml);
                    result.InnerHtml.AppendHtml(tag);
                }
                

            }

            output.TagName = "div";

            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}
