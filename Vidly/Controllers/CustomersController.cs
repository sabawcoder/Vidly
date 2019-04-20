﻿using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Vidly.ViewModels;

namespace Vidly.Models
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext context;
        public CustomersController()
        {
            context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }

        public ActionResult New()
        {
            var membershipTypes = context.MembershipTypes.ToList();
            var viewModel = new CustomerFormViewModel
            {
                MembershipTypes = membershipTypes
            };
            return View("CustomerForm", viewModel);
        }

        [HttpPost]
        public ActionResult Save(Customer customer)
        {
            if (customer.Id == 0)
                context.Customers.Add(customer);
            else
            {
                var customerInDb = context.Customers.Single(c => c.Id == customer.Id);
                customerInDb.Name = customer.Name;
                customerInDb.Birthdate = customer.Birthdate;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
                customerInDb.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
            }

            context.SaveChanges();

            return RedirectToAction("Index", "Customers");
        }

        public ViewResult Index()
        {
            var customers = context.Customers.Include(c => c.MembershipType).ToList();
            return View(customers);
        }

        public ActionResult Details(int id)
        {
            var customer = context.Customers.Include(c => c.MembershipType).ToList().SingleOrDefault(c => c.Id == id);
            return View(customer);
        }

        public ActionResult Edit(int id)
        {
            var customer = context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return HttpNotFound();

            var viewModel = new CustomerFormViewModel
            {
                Customer = customer,
                MembershipTypes = context.MembershipTypes.ToList()
            };
            return View("CustomerForm", viewModel);
        }
    }

}