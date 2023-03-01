using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Ecubytes.Identity.Login.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Ecubytes.AspNetCore.WebUtilities.Api;

namespace Ecubytes.Identity.Login.Web.Controllers
{
    public class AccountController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        private readonly ApiProfileManager apiProfileManager;
        public AccountController(
            ApiProfileManager apiProfileManager)
        {
            this.apiProfileManager = apiProfileManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null, string ecubytesapp = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ViewData["ReturnUrl"] = model.ReturnUrl;
            Identity.IdentityClient client = null;
            try
            {
                if (ModelState.IsValid)
                {
                    var apiProfile = apiProfileManager.Get("Identity.User");

                    client = new Identity.IdentityClient(
                        apiProfile.BaseAddress,
                        apiProfile.ClientId,
                        apiProfile.ClientSecret);

                    //Validate Credentials
                    var responseLogin = await client.LoginAsync(new Identity.Models.LoginRequest()
                    {
                        LogonName = model.Username,
                        Password = model.Password,
                        TenantId = GlobalOptions.DefaultTenantId
                    });

                    if (responseLogin.IsValid)
                    {
                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username)
                    };

                        var roles = await client.GetEffectiveRolesAsync(new Identity.Models.UserRoleRequest()
                        {
                            ApplicationId = GlobalOptions.DefaultApplicationId,
                            UserId = responseLogin.Data.UserId
                        });

                        //string.Join(" ", roles.Select(p=>p.CodeName).Distinct()
                        claims.AddRange(roles.Select(p => new Claim("role", p.CodeName)));

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));

                        //if (Url.IsLocalUrl(model.ReturnUrl))
                        //{
                        if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
                            return Redirect(model.ReturnUrl);
                        else
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        //}

                        //return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                    else
                        this.Messages.AddRange(responseLogin.Messages);
                }
            }
            catch (Exception ex)
            {
                this.AddErrorMessage(ex);
            }
            finally
            {
                if (client != null)
                    client.Dispose();
            }

            return View(model);
        }

        public IActionResult AccessDenied(string returnUrl)
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(scheme: "Cookies");

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
