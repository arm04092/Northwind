using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Configuration;

namespace Northwind.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
            {
                try
                {
                    connection.Open();
                    

                }
                catch(Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return View();
        }
    }
}