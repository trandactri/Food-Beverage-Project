using LoginandR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginandR.Controllers
{
    public class FeedbackController : Controller
    {
        DB_Entities _db = new DB_Entities();

        /// <summary>
        /// Action used to return a view of feedbacks list 
        /// </summary>
        /// <returns>If session role is supplier return a view of feedbacks list with suitable supID,
        /// else if session role is null return login page, 
        /// else redirect to create action</returns>
        public ActionResult Index()
        {
            if (Session["Role"] == null)
                return RedirectToAction("Login", "Home");
            if (Session["Role"].ToString() != "supplier")
                return RedirectToAction("Create");
            else
            {
                int tempID = Convert.ToInt32(Session["ID"]); 
                return View(_db.Feedbacks.Where(x => x.supID == tempID).ToList());
            }
                
        }

        /// <summary>
        /// Action used to create viewbag for supplier list and return a view
        /// </summary>
        /// <returns> a create view</returns>
        public ActionResult Create()
        {
            if (Session["Role"] == null)
                return RedirectToAction("Login", "Home");
            if (Session["Role"].ToString() == "supplier")
                return RedirectToAction("Index");
            ViewBag.SupList = new SelectList(_db.Suppliers.Where(x => x.supStatus == true),"supID","supName");
            return View();
        }

        /// <summary>
        /// After creating a new feedback, send values back to server for processing
        /// </summary>
        /// <param name="feedback">Feedback object after creating</param>
        /// <returns>If successfully create, return to User login page, else ask for trying again</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "fID,uID,supID,fMessage,fDate")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                _db.Feedbacks.Add(feedback);
                var temp = _db.Suppliers.Where(x => x.supID == feedback.supID).FirstOrDefault().supName;
                _db.SaveChanges();
                TempData["success"] = "Successfully send feedback to " + temp;
                return RedirectToAction("Index","Home");
            }            
            return View(feedback);
        }


        
    }
}