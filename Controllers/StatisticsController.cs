using LoginandR.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginandR.Controllers
{
    /// <summary>
    /// Statistics Controller
    /// </summary>
    public class StatisticsController : Controller
    {
        /// <summary>
        /// Action used to return index view
        /// </summary>
        /// <returns>Return view if session role is supplier, else return to login</returns>
        [HttpGet]
        // GET: Statistics
        public ActionResult Index()
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Supplier");
            }
            else
            {
                if (Session["Role"].ToString() != "supplier")
                {
                    return RedirectToAction("Login", "Supplier");
                }
            }
            return View();
        }

        /// <summary>
        /// Query datas, change into json form for later processing
        /// </summary>
        /// <returns>datas with json form</returns>
        public JsonResult GetPiechartJSON()
        {
            List<PieChartModel> list = new List<PieChartModel>();
            var temp = Convert.ToInt32(Session["ID"]);
            using (var context = new DB_Entities())
            {
                list = context.BillDetails.Where(f => f.Products.supID == temp).GroupBy(a => a.proName).Select( a => new PieChartModel { Quantity = a.Sum(b => b.quantity), Product = a.Key } ).ToList();
            }
            return Json(new { JSONList = list }, JsonRequestBehavior.AllowGet);
        }

    }
}