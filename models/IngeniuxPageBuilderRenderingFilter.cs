using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Ingeniux.Runtime.Models
{
	public class IngeniuxPageBuilderRenderingFilterAttribute : ActionFilterAttribute
	{
		public IngeniuxPageBuilderRenderingFilterAttribute() : base()
		{
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var sb = new StringBuilder();
			var sw = new StringWriter(sb);
			var tw = new HtmlTextWriter(sw);
			var output = (HttpWriter)filterContext.RequestContext.HttpContext.Response.Output;

			//hijack the content to write to string builder
			filterContext.RequestContext.HttpContext.Response.Output = tw;

			filterContext.HttpContext.Items["OriginalViewHtml"] = sb;
			filterContext.HttpContext.Items["OriginalViewOutput"] = output;
		}

		public override void OnResultExecuted(ResultExecutedContext filterContext)
		{
			StringBuilder sb = filterContext.HttpContext.Items["OriginalViewHtml"] as StringBuilder;

			if (sb != null)
			{
				//manipulate the string builder and dump back to output
				string originalHtml = sb.ToString();
				(filterContext.HttpContext.Items["OriginalViewOutput"] as HttpWriter).Write(sb.ToString() + "<test>怪壳壳</test>");
			}
		}
	}
}