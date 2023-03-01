using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecubytes.AspNetCore.Mvc.TagHelpers
{
    public static class TagBuilderExtensions
    {
        public static string RenderToString(this TagBuilder tag)
        {            
            using (var writer = new System.IO.StringWriter())
            {
                tag.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }
    }
}
