using System;
using System.Collections.Generic;

namespace Ecubytes.AspNetCore.Mvc.TagHelpers.Template
{
    public static class MessageBlockTemplate //: BaseElementTemplate
    {
        public static string PrimaryClass { get; set; } = "alert alert-primary";
        public static string SecondaryClass { get; set; } = "alert alert-secondary";
        public static string SuccessClass { get; set; } = "alert alert-success";
        public static string DangerClass { get; set; } = "alert alert-danger";
        public static string WarningClass { get; set; } = "alert alert-warning";
        public static string InfoClass { get; set; } = "alert alert-info";
        public static string LightClass { get; set; } = "alert alert-light";
        public static string DarkClass { get; set; } = "alert alert-dark";    
        public static string DismissibleClass { get; set; } = "alert-dismissible fade show";
        public static string ButtonCloseClass { get; set; } = "btn-close";
        public static Dictionary<string,string> ButtonCloseAttributes { get; } =  new Dictionary<string, string>()
        {
            ["data-bs-dismiss"] = "alert",
            ["aria-label"] = "Close",
            ["type"] = "button"
        };
        public static Dictionary<string,string> Attributes { get; } =  new Dictionary<string, string>()
        {
            ["role"] = "alert"
        };
    }
}
