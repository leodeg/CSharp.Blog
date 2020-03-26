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

			if (PagingInformation.HasPreviousPage)
			{
				TagBuilder previousPage = CreateTag(PagingInformation.CurrentPage - 1, urlHelper, "Previous");
				TagBuilder firstPage = CreateTag(1, urlHelper, "First Page");
				tag.InnerHtml.AppendHtml(firstPage);
				tag.InnerHtml.AppendHtml(previousPage);
			}

			if (PagingInformation.TotalPages <= 6)
			{
				CreatePagination(tag, urlHelper, 1, PagingInformation.TotalPages);
			}
			else if (PagingInformation.TotalPages > 6)
			{
				if (PagingInformation.CurrentPage <= 3)
					CreatePagination(tag, urlHelper, 1, 3);
				else if (PagingInformation.CurrentPage > 3)
					CreatePagination(tag, urlHelper, PagingInformation.CurrentPage - 3, PagingInformation.CurrentPage);

				int leftPages = PagingInformation.TotalPages - PagingInformation.CurrentPage;

				if (leftPages > 0 && leftPages <= 2)
					CreatePagination(tag, urlHelper, PagingInformation.CurrentPage + 1, PagingInformation.CurrentPage + leftPages);
				else if (PagingInformation.CurrentPage <= 3)
					CreatePagination(tag, urlHelper, PagingInformation.TotalPages - 2, PagingInformation.TotalPages);
				else if (leftPages > 0)
					CreatePagination(tag, urlHelper, PagingInformation.CurrentPage + 1, PagingInformation.CurrentPage + 3);
			}

			if (PagingInformation.HasNextPage)
			{
				TagBuilder nextPage = CreateTag(PagingInformation.CurrentPage + 1, urlHelper, "Next");
				TagBuilder lastPage = CreateTag(PagingInformation.TotalPages, urlHelper, "Last Page");
				tag.InnerHtml.AppendHtml(nextPage);
				tag.InnerHtml.AppendHtml(lastPage);
			}

			output.Content.AppendHtml(tag);
		}

		private void CreatePagination(TagBuilder tag, IUrlHelper urlHelper, int start, int end)
		{
			for (int i = start; i <= end; i++)
			{
				TagBuilder current = CreateTag(i, urlHelper);
				tag.InnerHtml.AppendHtml(current);
			}
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

			link.InnerHtml.Append(title ?? pageNumber.ToString());
			item.InnerHtml.AppendHtml(link);
			return item;
		}
	}
}
