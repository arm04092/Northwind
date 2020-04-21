using Northwind.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using System.IO;
using System.Data;
using PagedList;

namespace Northwind.Controllers
{
    public class EmployeesController : Controller
    {
        private NorthwindEntities1 db = new NorthwindEntities1();

        // GET: Employees
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            // sort param
            ViewBag.IDSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";
            ViewBag.CurrentSort = sortOrder;

            // search param
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var employees = from e in db.Employees
                            select e;
            // filtering
            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.LastName.Contains(searchString) || e.FirstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name":
                    employees = employees.OrderBy(e => e.LastName);
                    break;
                case "name_desc":
                    employees = employees.OrderByDescending(e => e.LastName);
                    break;
                case "Date":
                    employees = employees.OrderBy(e => e.HireDate);
                    break;
                case "date_desc":
                    employees = employees.OrderByDescending(e => e.HireDate);
                    break;
                case "id_desc":
                    employees = employees.OrderByDescending(e => e.EmployeeID);
                    break;
                default:
                    employees = employees.OrderBy(e => e.EmployeeID);
                    break;
            }
            // pagination
            int pageSize = 3;
            int pageNumber = (page ?? 1); // if page != null ? page : 1

            return View(employees.ToPagedList(pageNumber, pageSize));
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
                    if (employeesForm.BirthDate != null)
                        employees.BirthDate = Convert.ToDateTime(employeesForm.BirthDate);
                    employees.City = employeesForm.City;
                    employees.Country = employeesForm.Country;
                    employees.Extension = employeesForm.Extension;
                    employees.FirstName = employeesForm.FirstName;
                    if (employeesForm.HireDate != null) 
                        employees.HireDate = Convert.ToDateTime(employeesForm.HireDate);
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
            catch (DataException e)
            {
                System.Diagnostics.Trace.WriteLine("message:\n"+e.Message);
                if(e.GetBaseException() != null)
                    System.Diagnostics.Trace.WriteLine("inner exception message:\n" + e.GetBaseException().Message);
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

        // GET: Employees/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employees employees = db.Employees.Find(id);
            if( employees == null )
            {
                return HttpNotFound();
            }
            EmployeesForm employeesForm = new EmployeesForm();

            employeesForm.Address = employees.Address;
            // date formatting
            if (employees.BirthDate != null)
                employeesForm.BirthDate = employees.BirthDate.ToString().Substring(0,10);
            employeesForm.City = employees.City;
            employeesForm.Country = employees.Country;
            employeesForm.Extension = employees.Extension;
            employeesForm.FirstName = employees.FirstName;
            // date formatting
            if (employees.HireDate != null)
                employeesForm.HireDate = employees.HireDate.ToString().Substring(0, 10);
            employeesForm.HomePhone = employees.HomePhone;
            employeesForm.LastName = employees.LastName;
            employeesForm.Notes = employees.Notes;
            employeesForm.PostalCode = employees.PostalCode;
            employeesForm.Region = employees.Region;
            employeesForm.ReportsTo = employees.ReportsTo;
            employeesForm.Title = employees.Title;
            employeesForm.TitleOfCourtesy = employees.TitleOfCourtesy;
            /*
            if (employees.Photo != null)
                employeesForm.Photo = (HttpPostedFileBase)new MemoryPostedFile(employees.Photo);
            */
            ViewBag.ReportsTo = new SelectList(db.Employees, "ReportsTo", "LastName");
            return View(employeesForm);
        }

        // POST: Employees/Edit
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            System.Diagnostics.Trace.WriteLine("id:" + id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var EmployeesToUpdate = db.Employees.Find(id);
            if (TryUpdateModel(EmployeesToUpdate, "",
                new string[] { "LastName", "FirstName", "Title", "TitleOfCourtesy", "BirthDate", "HireDate", "Address", "City", "Region", "PostalCode", "Country", "HomePhone", "Extension","Notes", "ReportsTo" }))
            {
                try
                {
                    
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch(DataException /* dex */)
                {
                    //  
                    // Loging
                    //
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            return View(EmployeesToUpdate);
        }
    }

    public class MemoryPostedFile : HttpPostedFileBase
    {
        private readonly byte[] fileBytes;

        public MemoryPostedFile(byte[] fileBytes, string fileName = null)
        {
            this.fileBytes = fileBytes;
            this.FileName = fileName;
            this.InputStream = new MemoryStream(fileBytes);
        }

        public override int ContentLength => fileBytes.Length;

        public override string FileName { get; }

        public override Stream InputStream { get; }
    }

}