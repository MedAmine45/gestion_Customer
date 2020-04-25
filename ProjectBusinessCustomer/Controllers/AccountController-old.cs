using ProjectBusinessCustomer.Infrastructure.Abstract;
using ProjectBusinessCustomer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace ProjectBusinessCustomer.Controllers
{
    public class AccountController : Controller
    {
        //private ApplicationSignInManager _signInManager;
        //private ApplicationUserManager _userManager;
        IAuthProvider authProvider;

        public AccountController()
        {

        }

        public AccountController(IAuthProvider auth)
        {
            this.authProvider = auth;
        }
        public ViewResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (authProvider.Authenticate(model.Email, model.Password))
                    return Redirect(returnUrl ?? Url.Action("Societies", "Society"));
                else
                {
                    ModelState.AddModelError("", "Incorrect Username/Password");
                    return View();
                }
            }
            else
                return View();
        }

    }
}