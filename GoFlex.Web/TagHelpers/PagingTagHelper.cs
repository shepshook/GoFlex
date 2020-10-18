using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using GoFlex.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GoFlex.Web.TagHelpers
{
    public class PagingTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public PagingTagHelper(IUrlHelperFactory helperFactory) =>
            _urlHelperFactory = helperFactory;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "nav";

            var tag = new TagBuilder("ul");
            tag.AddCssClass("pagination");

            var items = new List<TagBuilder>();

            var prev = CreateItem(PageViewModel.Current - 1, urlHelper, "&#128072;");
            if (!PageViewModel.HasPrevious)
                prev.AddCssClass("disabled");
            items.Add(prev);

            if (PageViewModel.Total <= 6)
                items.AddRange(Enumerable.Range(1, PageViewModel.Total).Select(x => CreateItem(x, urlHelper)));
            else if (PageViewModel.Current <= 3)
            {
                items.AddRange(Enumerable.Range(1, 5).Select(x => CreateItem(x, urlHelper)));
                items.Add(CreateItem(null, urlHelper, "..."));
                items.Add(CreateItem(PageViewModel.Total, urlHelper));
            }
            else if (PageViewModel.Total - PageViewModel.Current <= 2)
            {
                items.Add(CreateItem(1, urlHelper));
                items.Add(CreateItem(null, urlHelper, "..."));
                items.AddRange(Enumerable.Range(PageViewModel.Total - 4, 5).Select(x => CreateItem(x, urlHelper)));
            }
            else
            {
                items.Add(CreateItem(1, urlHelper));
                if (PageViewModel.Current > 4)
                    items.Add(CreateItem(null, urlHelper, "..."));

                items.AddRange(Enumerable.Range(PageViewModel.Current - 2, 5).Select(x => CreateItem(x, urlHelper)));

                if (PageViewModel.Current < PageViewModel.Total - 3)
                    items.Add(CreateItem(null, urlHelper, "..."));
                items.Add(CreateItem(PageViewModel.Total, urlHelper));
            }

            var next = CreateItem(PageViewModel.Current + 1, urlHelper, "&#128073;");
            if (!PageViewModel.HasNext)
                next.AddCssClass("disabled");
            items.Add(next);

            foreach (var item in items)
                tag.InnerHtml.AppendHtml(item);
            output.Content.AppendHtml(tag);
        }

        private TagBuilder CreateItem(int? page, IUrlHelper urlHelper, string text = null)
        {
            var item = new TagBuilder("li");
            item.AddCssClass("page-item");

            var itemLink = new TagBuilder("a");
            itemLink.AddCssClass("page-link");

            if (!page.HasValue)
                item.AddCssClass("disabled");
            else if (page.Value == PageViewModel.Current)
                item.AddCssClass("active");
            else
                itemLink.Attributes["href"] = urlHelper.Action(Action, Controller, GetParameters(page.Value));

            itemLink.InnerHtml.AppendHtml(text ?? page.ToString());
            item.InnerHtml.AppendHtml(itemLink);

            return item;
        }

        private object GetParameters(int page)
        {
            var result = new ExpandoObject();
            var props = (ICollection<KeyValuePair<string, object>>)result;

            props.Add(new KeyValuePair<string, object>("page", page));

            foreach (var param in PageViewModel.Parameters)
                props.Add(new KeyValuePair<string, object>(param.Key, param.Value));

            return result;
        }
    }
}
