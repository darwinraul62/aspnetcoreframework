using System;
using Ecubytes.Data;

namespace Ecubytes.AspNetCore.Mvc.Models
{
    public class JsonModelResult : ModelResult
    {
        public string RedirectUrl { get; set; }
    }

    public class JsonModelResult<T> : ModelResult<T>
    {
        public string RedirectUrl { get; set; }
    }
}
