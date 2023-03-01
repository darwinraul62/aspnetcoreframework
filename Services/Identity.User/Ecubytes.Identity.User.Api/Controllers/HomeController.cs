using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Ecubytes.Identity.User.Api.Controllers
{
    public class HomeController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
