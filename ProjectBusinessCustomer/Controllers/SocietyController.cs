using ProjectBusinessCustomer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectBusinessCustomer.Controllers
{
   
    public class SocietyController : Controller
    {
        public MyDbContext myDbContext = new MyDbContext();
        public SocietyController()
        {

        }
        public SocietyController(MyDbContext db)
        { 
            this.myDbContext = db;
        }

        // GET: Society

        public ActionResult Societies()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", new RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }
            IEnumerable<Society> societies = myDbContext.Societies.ToList();
            return View("Societies", societies);
        }

        public ActionResult FormSociety()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveSociety(Society s)
        {
            if (ModelState.IsValid)
            {
                myDbContext.Societies.Add(s);
                myDbContext.SaveChanges();

            }
            return View("FormSociety", s);
        }

        public ActionResult Delete(int SocietyID)
        {
            Society society = myDbContext.Societies.FirstOrDefault(s => s.SocietyID == SocietyID);
            if (society != null)
            {
                myDbContext.Societies.Remove(society);
                myDbContext.SaveChanges();
            }
            IEnumerable<Society> societiesUpdate = myDbContext.Societies.ToList();
            return View("Societies", societiesUpdate);
        }
        public ActionResult Edit(int SocietyID)
        {
            Society society = myDbContext.Societies.FirstOrDefault(s => s.SocietyID == SocietyID);
            return View(society);
        }
        [HttpPost]
        public ActionResult Edit(Society s)
        {
            if (ModelState.IsValid)
            {
                Society society = myDbContext.Societies.Where(x => x.SocietyID == s.SocietyID).FirstOrDefault();
                society.SocietyName = s.SocietyName;
                society.CompanySocial = s.CompanySocial;
                society.SocietyAddress = s.SocietyAddress;
                society.SocietyEmail = s.SocietyEmail;
                society.SocietyPhone = s.SocietyPhone;
                society.WebSite = s.WebSite;
                society.SocietyFax = s.SocietyFax;
                myDbContext.SaveChanges();
            }
            return View();
        }

        public ActionResult Details(int SocietyID)
        {
            Society society = myDbContext.Societies.FirstOrDefault(s => s.SocietyID == SocietyID);

            IEnumerable<Society_Projects> society_Projects = from sp in myDbContext.Society_Projects.ToList()
                                                             where
                                                             sp.SocietyID == SocietyID
                                                             select sp;


                    IEnumerable<Project> Allprojects = myDbContext.Projects.ToList();
                    var projects =
                             from p in Allprojects
                             join sp in society_Projects on p.ProjectID equals sp.ProjectID
                             select p;

            ViewBag.Projects = projects;
            return View(society);
        }



        public ActionResult FormProject(int SocietyID)
        {
            Society society = myDbContext.Societies.FirstOrDefault(s => s.SocietyID == SocietyID);
            ViewBag.Projects = myDbContext.Projects.ToList();
            TempData["SocietyID"] = SocietyID.ToString();
            return View(society);
        }
        [HttpPost]
        public ActionResult SaveProject(int ProjectID)
        {
            int idSociete = int.Parse(TempData["SocietyID"].ToString());
            if (!myDbContext.Society_Projects.Any(s => s.ProjectID == ProjectID && s.SocietyID == idSociete))
            {
                Society_Projects society_Projects = new Society_Projects();

                society_Projects.ProjectID = ProjectID;
                society_Projects.SocietyID = idSociete;
                myDbContext.Society_Projects.Add(society_Projects);
                myDbContext.SaveChanges();
            }
            ViewBag.Projects = myDbContext.Projects.ToList();
            return RedirectToAction("Societies");
        }
        
        public ActionResult Remove(int ProjectID)
        {
            Society_Projects society_Project = myDbContext.Society_Projects.FirstOrDefault(s => s.ProjectID == ProjectID);
            if(society_Project != null)
            {
                myDbContext.Society_Projects.Remove(society_Project);
                myDbContext.SaveChanges();

            }

            IEnumerable<Society_Projects> society_Projects = from sp in myDbContext.Society_Projects.ToList()
                                                             where
                                                             sp.SocietyID == society_Project.SocietyID
                                                             select sp;


            IEnumerable<Project> Allprojects = myDbContext.Projects.ToList();
            var projects =
                     from p in Allprojects
                     join sp in society_Projects on p.ProjectID equals sp.ProjectID
                     select p;

            ViewBag.Projects = projects;

            
            Society society = myDbContext.Societies.FirstOrDefault(s => s.SocietyID == society_Project.SocietyID);
            return View("Details", society);
        }
    }
}