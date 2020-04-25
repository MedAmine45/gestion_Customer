using ProjectBusinessCustomer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectBusinessCustomer.Controllers
{
    public class ProjectController : Controller
    {
        ExecuteQuery Query = new ExecuteQuery();
        MyDbContext MyDbContext = new MyDbContext();
        public string endPoint = "https://inlsprl.visualstudio.com/DefaultCollection/_apis/projects?stateFilter=All";
        // GET: Project
        public ActionResult Projects()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", new RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }
            List<Project> list = Query.GetProjects(endPoint);
            Query.synchronisationProject(list);
            List<Project> projects = MyDbContext.Projects.ToList();
            return View(projects);
        }

        public ActionResult Details(int ProjectID)
        {
            Project project = MyDbContext.Projects.FirstOrDefault(p => p.ProjectID == ProjectID);
            return View(project);
        }

        [HttpPost]
        public ActionResult Details(Project p)
        {
                Project project = MyDbContext.Projects.Where(x => x.ProjectID == p.ProjectID).FirstOrDefault();
                MyDbContext.SaveChanges();
              return View();
        }
    }
}