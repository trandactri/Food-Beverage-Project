using LoginandR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginandR.Controllers
{
    /// <summary>
    /// Controller for Bill
    /// </summary>
    public class BillController : Controller
    {
        /// <summary>
        /// Variable used for activities on DB_Entities
        /// </summary>
        private readonly DB_Entities _db = new DB_Entities();
        
        /// <summary>
        /// Action used to return a view
        /// </summary>
        /// <returns>A view</returns>
        public ActionResult Index()
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (Session["Role"].ToString() != "user")
                {
                    return RedirectToAction("Login", "Home");
                }
            }           
            List<Cart> lstCart = GetBill();
            ViewBag.stringTotal = TotalPrice().ToString("#,##");
            ViewBag.total = TotalPrice();
            return View(lstCart);
        }

        /// <summary>
        /// Function used to get and save bill to session
        /// </summary>
        /// <returns>Return a list cart if have cart session, else create new cart session</returns>
        public List<Cart> GetBill()
        {
            //gio hang da ton tai
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart == null)
            {
                //Neu session gio hang chua ton tai thi khoi tao gio hang
                lstCart = new List<Cart>();
                Session["Cart"] = lstCart;
            }
            return lstCart;
        }

        /// <summary>
        /// Function used to calculate total price
        /// </summary>
        /// <returns>Summary of cart price</returns>
        public double TotalPrice()
        {
            //Lay gio hang
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart == null)
            {
                return 0;
            }
            return lstCart.Sum(n => n.cartPrice);
        }

        /// <summary>
        /// Action used to checkout after user confirm items
        /// </summary>
        /// <returns>clear cart session and return to bill index page with successful message</returns>
        [HttpPost]
        //Xay dung chuc nang dat hang
        public ActionResult Checkout()
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (Session["Role"].ToString() != "user")
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            //Kiem tra session gio hang ton tai chua
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            //Them don hang
            Bill dh = new Bill();
            dh.uID = Convert.ToInt32(Session["ID"]);
            dh.bDate = DateTime.Now;
            dh.deliStatus = true;
            dh.paid = false;
            dh.caceled = false;
            dh.deleted = false;
            dh.shipDate = DateTime.Now.AddMinutes(15);
            _db.Bills.Add(dh);
            _db.SaveChanges();

            //Them chi tiet don dat hang
            List<Cart> lstGH = GetBill();
            foreach (var item in lstGH)
            {
                BillDetail det = new BillDetail();
                det.bID = dh.bID;
                TempData["order"] = det.bID;
                det.proID = item.proID;
                det.proName = item.proName;
                det.quantity = item.cartQuantity;
                det.price = item.proPrice * item.cartQuantity;
                _db.BillDetails.Add(det);
            }
            _db.SaveChanges();
            Session["Cart"] = null;
            TempData["modalValid"] = "yes";
            return RedirectToAction("Index", "Bill");
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