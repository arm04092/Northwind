using Northwind.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using System.IO;

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
                if(id<10)
                {
                    ms.Write(image, 78, image.Length - 78);
                }
                else
                {
                    ms.Write(image, 0, image.Length);
                }

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
        // id와 path는 바인딩 안함
        public ActionResult Create([Bind(Include = "LastName,FirstName,Title,TitleOfCourtesy,BirthDate,HireDate,Address,City,Region,PostalCode,Country,HomePhone,Extension,Photo,Notes,ReportsTo")]
                                    EmployeesForm employeesForm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.ReportsTo = new SelectList(db.Employees, "ReportsTo", "LastName", employeesForm.ReportsTo);
                    Employees employees = new Employees();
                    if ( employeesForm.Photo != null && employeesForm.Photo.ContentLength > 0 && employeesForm.Photo.ContentType.Contains("image"))
                    {
                        MemoryStream target = new MemoryStream();
                        employeesForm.Photo.InputStream.CopyTo(target);
                        byte[] data = target.ToArray();
                        employees.Photo = data;
                        employees.PhotoPath = Path.Combine(Server.MapPath("~App_Data/photo"), employeesForm.Photo.FileName);
                    }
                    employees.Address = employeesForm.Address;
                    employees.BirthDate = employeesForm.BirthDate;
                    employees.City = employeesForm.City;
                    employees.Country = employeesForm.Country;
                    employees.Extension = employeesForm.Extension;
                    employees.FirstName = employeesForm.FirstName;
                    employees.HireDate = employeesForm.HireDate;
                    employees.HomePhone = employeesForm.HomePhone;
                    employees.LastName = employeesForm.LastName;                    
                    employees.Notes = employeesForm.Notes;
                    employees.PostalCode = employeesForm.PostalCode;
                    employees.Region = employeesForm.Region;
                    employees.ReportsTo = employeesForm.ReportsTo;
                    employees.Title = employeesForm.Title;
                    employees.TitleOfCourtesy = employeesForm.TitleOfCourtesy;
                    db.Employees.Add(employees);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine("message:\n"+e.Message);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(employeesForm);
        }

        // GET: Employees/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employees employees = db.Employees.Find(id);
            if (employees == null)
            {
                return HttpNotFound();
            }

            return View(employees);
        }

        // POST: Employees/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employees employees = db.Employees.Find(id);
            db.Employees.Remove(employees);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}