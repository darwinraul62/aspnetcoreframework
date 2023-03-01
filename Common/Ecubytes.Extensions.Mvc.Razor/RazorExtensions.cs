using System;
using Ecubytes.Data;

namespace Microsoft.AspNetCore.Mvc.Razor
{    
    public static class RazorPageExtensions
    {
        public static MessageColletion Messages(this RazorPage razorPage) => razorPage.ViewBag._Messages as MessageColletion;        
    }
}