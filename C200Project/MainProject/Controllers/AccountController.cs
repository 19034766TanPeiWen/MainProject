using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MainProject.Models;
using System;
using System.Data;
using System.Security.Claims;

namespace MainProject.Controllers
{
   public class AccountController : Controller
   {
      [Authorize]
      public IActionResult Logoff(string returnUrl = null)
      {
         HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
         if (Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);
         return RedirectToAction("homepage", "Main");
      }

      [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            TempData["ReturnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(UserLogin user)
        {
            if (!AuthenticateUser(user.UserName, user.Password,
                                  out ClaimsPrincipal principal))
            {
                ViewData["Message"] = "Incorrect User Name or Password";
                return View();
            }
            else
            {
                HttpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   principal);

                if (TempData["returnUrl"] != null)
                {
                    string returnUrl = TempData["returnUrl"].ToString();
                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                }

                return RedirectToAction("profile", "Main");
            }
        }


        private bool AuthenticateUser(string uid, string pw,
                                    out ClaimsPrincipal principal)
      {
         principal = null;

         string sql = @"SELECT * FROM UserDetails
                         WHERE UserName = '{0}' AND Password= '{1}'";
         string select = String.Format(sql, uid, pw);
         DataTable ds = DBUtl.GetTable(select);
         if (ds.Rows.Count == 1)
         {
            principal =
               new ClaimsPrincipal(
                  new ClaimsIdentity(
                     new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier, uid),
                        new Claim(ClaimTypes.Name, ds.Rows[0]["UserName"].ToString()),
                     },
                     CookieAuthenticationDefaults.AuthenticationScheme));
            return true;
         }
         return false;
      }

   }
}