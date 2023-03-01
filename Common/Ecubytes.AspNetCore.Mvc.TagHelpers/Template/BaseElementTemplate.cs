using System;
using System.Collections.Generic;

namespace Ecubytes.AspNetCore.Mvc.TagHelpers.Template
{
    public class BaseElementTemplate
    {        
        public BaseElementTemplate()
        {
               
        }
        
        public Dictionary<string,string> Attributes { get; } = new Dictionary<string, string>();
    }
}
