using System;
using Ecubytes.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecubytes.Identity.Login.Web.Controllers
{    
    public class HomeController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {            
            return View();
        }

        [HttpGet]
        //[ClaimAuthorize("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Customer.Write")]
        [Authorize(Roles = "Customer.Write", AuthenticationSchemes = "OpenIdConnect")]
        // [Authorize(Roles = "Customer.Write,Customer.Read,All", AuthenticationSchemes = "OpenIdConnect")]
        //[ScopeAuthorize("openid")]
        public IActionResult Privacy()
        {            
            return View();
        }
    }
}
