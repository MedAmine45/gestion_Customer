using ProjectBusinessCustomer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace ProjectBusinessCustomer.Controllers
{
    public class WorkItemController : Controller
    {
        ExecuteQuery Query = new ExecuteQuery();
        MyDbContext myDbContext = new MyDbContext();
        private ApplicationDbContext db = new ApplicationDbContext();
        public WorkItemController()
        {

        }
        public WorkItemController(MyDbContext db)
        {
            this.myDbContext = db;
        }
        public WorkItemController(MyDbContext db, ExecuteQuery query)
        {
            this.myDbContext = db;
            this.Query = query;
        }
        // GET: WorkItem
        public ActionResult Tasks()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", new RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }
            Query.synchronisation();
            IEnumerable<Project> projects = myDbContext.Projects.ToList();// récuperer la liste des projets pour avoir une liste déroulante
            ViewBag.Projects = projects;// et  le stocker dans le modéle
            List<Task> tasks = myDbContext.Tasks.ToList();
           
            
            //string currentUserId = User.Identity.GetUserId();
            //ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            //string userName = currentUser.Email;
            //IEnumerable<Task> tasksassignee = from t in tasks
            //                                               where
            //                                               t.userEmail == currentUser.Email
            //                           select t;
            return View(tasks);
        }
        public ActionResult Synchronisation()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", new RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }
            ViewBag.Projects = myDbContext.Projects.ToList();
            ViewBag.Name = new SelectList(myDbContext.Projects.ToList(), "Name", "Name");

            return View();
        }
        [HttpPost]
        public ActionResult Synchronisation(UserToken model)
        {
            UserToken userToken = new UserToken();
            //ViewBag.Projects = myDbContext.Projects.ToList();
            //ViewBag.Name = new SelectList(myDbContext.Projects.ToList(), "Name", "Name");
            if (ModelState.IsValid)
            {
                userToken.Uri = model.Uri;
                userToken.Token = model.Token;
                userToken.Projectname = model.Projectname;   
                //myDbContext.Tokens.Add(userToken);
                //myDbContext.SaveChanges();
                ExecuteQuery query = new ExecuteQuery(userToken.Uri, userToken.Token, userToken.Projectname);
                
                query.synchronisation();
                return RedirectToAction("tasks");

            }
            else
            {
                return View();
            }
            
           
        }
    }
}