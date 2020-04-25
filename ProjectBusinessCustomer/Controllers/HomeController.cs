using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ProjectBusinessCustomer.Models;
using ProjectBusinessCustomer.TokenStorage;

namespace ProjectBusinessCustomer.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //public ActionResult Index()
        //{
        //    return RedirectToAction("IndexAsync");
        //}
        [Authorize]
        public async Task<ActionResult> Index()
        {
            if (Request.IsAuthenticated)
            {
                //string token = await SampleAuthProvider.Instance.GetUserAccessTokenAsync();

                //if (string.IsNullOrEmpty(token))
                //{
                //    // If there's no token in the session, redirect to Home
                //    return RedirectToAction("Login", "Account");
                //}


                string userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
                System.Diagnostics.Debug.WriteLine("Info :", ClaimsPrincipal.Current);
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                Session["UserID"] = userId;
                Session["UserName"] = currentUser.FirstName + " " + currentUser.LastName;
                return RedirectToAction("Customers", "Customer");

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        //public async System.Threading.Tasks.Task<ActionResult> test()
        //{
        //    VstsProjects oPlist = new VstsProjects();
        //    await oPlist.GetProjectsItem();
        //    return View();
        //}
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }


}