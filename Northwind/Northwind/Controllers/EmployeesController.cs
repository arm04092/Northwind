using Northwind.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;

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

        // GET: Employees/Details/{id}
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employees employees = db.Employees.Find(id);
            if(employees == null)
            {
                return HttpNotFound();
            }
            return View(employees);
        }

        // Get image file from employees.photo
        public ActionResult GetImg(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                byte[] image = db.Employees.Where(m => m.EmployeeID == id).Select(m => m.Photo).SingleOrDefault();

                // 78 is the size of the OLE header for Northwind images
                ms.Write(image, 78, image.Length - 78);

                return File(ms.ToArray(), "image/jpeg");
            }
        }
        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.ReportsTo = new SelectList(db.Employees, "ReportsTo", "LastName");

            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,LastName,FirstName,Title,TitleOfCourtesy,BirthDate,HireDate,Address,City,Region,PostalCode,Country,HomePhone,Extension,Photo,Notes,ReportsTo,PhotoPath")]
                                    Employees employees)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Employees.Add(employees);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.ReportsTo = new SelectList(db.Employees, "ReportsTo", "LastName", employees.ReportsTo);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine("message:\n"+e.Message);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(employees);
        }
    }
}