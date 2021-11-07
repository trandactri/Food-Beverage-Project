using LoginandR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginandR.Controllers
{
    public class BillController : Controller
    {
        private readonly DB_Entities _db = new DB_Entities();
        
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
            dh.deliStatus = false;
            dh.paid = false;
            dh.caceled = false;
            dh.deleted = false;
            _db.Bills.Add(dh);
            _db.SaveChanges();

            //Them chi tiet don dat hang
            List<Cart> lstGH = GetCart();
            foreach (var item in lstGH)
            {
                BillDetail det = new BillDetail();
                det.bID = dh.bID;
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

        public List<Cart> GetCart()
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
    }
}