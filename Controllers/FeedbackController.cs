using LoginandR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LoginandR.Controllers
{
    /// <summary>
    /// Controller for feedback
    /// </summary>
    public class FeedbackController : Controller
    {
        /// <summary>
        /// Variable used for activities on DB_Entities
        /// </summary>
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
                int tempID = Convert.ToInt32(Session["ID"]); 
                return View(_db.Feedbacks.Where(x => x.supID == tempID && x.Supplier.supStatus).ToList());   
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
            ViewBag.SupList = _db.Suppliers.Where(x => x.supStatus == true);
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
            if (feedback == null)
            {
                return HttpNotFound();
            }
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

        /// <summary>
        /// Action used to return a view of feedback details
        /// </summary>
        /// <param name="id">feedback id</param>
        /// <returns>a view of id matched feedback</returns>
        public ActionResult Details(int? id)
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = _db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        /// <summary>
        /// Action used to delete feedback based on fid
        /// </summary>
        /// <param name="id">feedback id</param>
        /// <returns>index page</returns>
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = _db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            feedback.fStatus = false;
            /*_db.Products.Remove(product);*/
            this._db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Function used to restore unneccessary or unused resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            // need to alway test if disposing pass else reallocations could occur during Finalize pass
            // also good practice to test resource was created
            if (disposing && _db != null)
                _db.Dispose();
            base.Dispose(disposing);
        }
    }
}