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
                from count in db.Employees
                select count
                ).Count();

                rows.Add(CountQuery);

                CountQuery = (
                from count in db.Categories
                select count
                ).Count();

                rows.Add(CountQuery);

                CountQuery = (
                from count in db.Customers
                select count
                ).Count();

                rows.Add(CountQuery);

                CountQuery = (
                from count in db.Shippers
                select count
                ).Count();

                rows.Add(CountQuery);

                CountQuery = (
                from count in db.Suppliers
                select count
                ).Count();

                rows.Add(CountQuery);

                CountQuery = (
                from count in db.Orders
                select count
                ).Count();

                rows.Add(CountQuery);

                CountQuery = (
                from count in db.Products
                select count
                ).Count();

                rows.Add(CountQuery);

                CountQuery = (
                from count in db.Order_Details
                select count
                ).Count();

                rows.Add(CountQuery);

                CountQuery = (
                from count in db.Region
                select count
                ).Count();

                rows.Add(CountQuery);

                CountQuery = (
                from count in db.Territories
                select count
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