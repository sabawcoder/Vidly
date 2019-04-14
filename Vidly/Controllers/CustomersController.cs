using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Vidly.Models
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext context;
        public CustomersController()
        {
            context = new ApplicationDbContext();
        }

        public ViewResult Index()
        {
            var customers = context.Customers.Include(c => c.MembershipType).ToList();
            return View(customers);
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }

        public ActionResult Details(int id)
        {
            var customer = context.Customers.Include(c => c.MembershipType).ToList().SingleOrDefault(c => c.Id == id);
            return View(customer);
        }
    }

}