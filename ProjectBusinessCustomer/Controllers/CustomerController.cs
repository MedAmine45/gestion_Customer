using ProjectBusinessCustomer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectBusinessCustomer.Controllers
{
    public class CustomerController : Controller
    {
        public MyDbContext myDbContext = new MyDbContext();
        public CustomerController()
        {

        }
        public CustomerController(MyDbContext db)
        {
            this.myDbContext = db;
        }
        // GET: Customer
        public ActionResult Customers()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", new RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }
            IEnumerable<Customer> customers =
                 myDbContext.
                 Customers
                 .ToList();
            return View("Customers", customers);
        }
        public ActionResult FormCustomer()
        {
            Customer c = new Customer();
            IEnumerable<Society> societies = myDbContext.Societies.ToList(); // récuperer la liste des societies pour avoir une liste déroulante
            ViewBag.Societies = societies;// et  le stocker dans le modéle
            return View("FormCustomer", c);
        }
        [HttpPost]
        public ActionResult SaveCustomer(Customer c)
        {
            IEnumerable<Society> societies = myDbContext.Societies.ToList();// récuperer la liste des societies pour avoir une liste déroulante
            ViewBag.Societies = societies;// et  le stocker dans le modéle
            if (ModelState.IsValid)
            {
                myDbContext.Customers.Add(c);
                myDbContext.SaveChanges();
            }

            return View("FormCustomer", c);
        }

        public ActionResult Delete(int CustomerID)
        {

            Customer customer = myDbContext.Customers.Where(c => c.CustomerID == CustomerID).FirstOrDefault();
            myDbContext.Customers.Remove(customer);
            myDbContext.SaveChanges();
            IEnumerable<Customer> customerUpdate = myDbContext.Customers.ToList();
            return View("Customers", customerUpdate);
        }

        public ActionResult Edit(int CustomerID)
        {
            Customer customer = myDbContext.Customers.FirstOrDefault(c => c.CustomerID == CustomerID);
            IEnumerable<Society> societies = myDbContext.Societies.ToList();
            ViewBag.Societies = societies;
            return View(customer);
        }

        [HttpPost]
        public ActionResult Edit(Customer c)
        {
            IEnumerable<Society> societies = myDbContext.Societies.ToList();
            ViewBag.Societies = societies;
            if (ModelState.IsValid)
            {
                Customer customer = myDbContext.Customers.FirstOrDefault(x => x.CustomerID == c.CustomerID);
                customer.CustomerName = c.CustomerName;
                customer.Profile = c.Profile;
                customer.Country = c.Country;
                customer.Region = c.Region;
                customer.Address = c.Address;
                customer.Email = c.Email;
                customer.Phone = c.Phone;
                customer.Fax = c.Fax;
                customer.Activity = c.Activity;
                customer.ImportantNotes = c.ImportantNotes;
                customer.SocietyID = c.SocietyID;
                myDbContext.SaveChanges();
            }
            return View();
        }
    }
}