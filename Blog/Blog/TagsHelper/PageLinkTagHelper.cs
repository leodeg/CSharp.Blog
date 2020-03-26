using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.TagsHelper
{
	[HtmlTargetElement("div", Attributes = "paging-information")]
	public class PageLinkTagHelper : TagHelper
	{
		private IUrlHelperFactory urlHelperFactory;

		public PageLinkTagHelper(IUrlHelperFactory helperFactory)
		{
			urlHelperFactory = helperFactory;
		}

		[ViewContext]
		[HtmlAttributeNotBound]
		public ViewContext ViewContext { get; set; }
		public PagingInformation PagingInformation { get; set; }
		public string PageAction { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
			output.TagName = "div";

			TagBuilder tag = new TagBuilder("ul");
			tag.AddCssClass("pagination");

			TagBuilder currentItem = CreateTag(PagingInformation.CurrentPage, urlHelper);

			if (PagingInformation.HasPreviousPage)
			{
				TagBuilder previousItem = CreateTag(PagingInformation.CurrentPage - 1, urlHelper, "Previous");
				tag.InnerHtml.AppendHtml(previousItem);
			}

			tag.InnerHtml.AppendHtml(currentItem);

			if (PagingInformation.HasNextPage)
			{
				TagBuilder nextItem = CreateTag(PagingInformation.CurrentPage + 1, urlHelper, "Next");
				tag.InnerHtml.AppendHtml(nextItem);
			}

			output.Content.AppendHtml(tag);
		}

		private TagBuilder CreateTag(int pageNumber, IUrlHelper urlHelper, string title = null)
		{
			TagBuilder item = new TagBuilder("li");
			TagBuilder link = new TagBuilder("a");

			if (pageNumber == this.PagingInformation.CurrentPage)
				item.AddCssClass("active");
			else
				link.Attributes["href"] = urlHelper.Action(PageAction, new { page = pageNumber });

			item.AddCssClass("page-item");
			link.AddCssClass("page-link");

			link.InnerHtml.Append(title?? pageNumber.ToString());
			item.InnerHtml.AppendHtml(link);
			return item;
		}
	}
}
