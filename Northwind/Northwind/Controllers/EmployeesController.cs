using Northwind.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Northwind.Controllers
{
    public class EmployeesController : Controller
    {
        private NorthwindEntities1 db = new NorthwindEntities1();

        // GET: Employees
        public ActionResult Index()
        {
            
            return View(db.Employees.ToList());
        }
    }
}