using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Linq;

using System.Data.Entity;
using Northwind.Models;

namespace Northwind.Controllers
{
    public class HomeController : Controller
    {
        private NorthwindEntities1 db = new NorthwindEntities1();

        public ActionResult Index()
        {
            
            try
            {
                List<string> names = new List<string>();
                
                names.Add("Employees");
                names.Add("Categories");
                names.Add("Customers");
                names.Add("Shippers");
                names.Add("Suppliers");
                names.Add("Orders");
                names.Add("Products");
                names.Add("Order Details");
                names.Add("Region");
                names.Add("Territories");

                List<int> rows = new List<int>();

                
                var CountQuery = (
                from customer in db.Employees
                select customer
                ).Count();

                rows.Add(CountQuery);

                CountQuery = (
                from customer in db.Categories
                select customer
                ).Count();

                rows.Add(CountQuery);

                CountQuery = (
                from customer in db.Customers
                select customer
                ).Count();

                rows.Add(CountQuery);
                CountQuery = (
                from customer in db.Shippers
                select customer
                ).Count();

                rows.Add(CountQuery);
                CountQuery = (
                from customer in db.Suppliers
                select customer
                ).Count();

                rows.Add(CountQuery);
                CountQuery = (
                from customer in db.Orders
                select customer
                ).Count();

                rows.Add(CountQuery);
                CountQuery = (
                from customer in db.Products
                select customer
                ).Count();

                rows.Add(CountQuery);
                CountQuery = (
                from customer in db.Order_Details
                select customer
                ).Count();

                rows.Add(CountQuery);
                CountQuery = (
                from customer in db.Region
                select customer
                ).Count();

                rows.Add(CountQuery);
                CountQuery = (
                from customer in db.Territories
                select customer
                ).Count();

                rows.Add(CountQuery);

                ViewBag.Names = names.ToArray();
                ViewBag.Rows = rows.ToArray();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
            }

            return View();
        }
    }
}