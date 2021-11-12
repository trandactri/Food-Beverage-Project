using LoginandR.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace LoginandR.Controllers
{
    /// <summary>
    /// Controller for Cart
    /// </summary>
    public class CartController : Controller
    {
        /// <summary>
        /// Variable used for activities on DB_Entities
        /// </summary>
        private DB_Entities _db = new DB_Entities();


        /// <summary>
        /// Function used to get cart
        /// </summary>
        /// <returns> return 1stCart </returns>
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




        /// <summary>
        /// Action used to add cart 
        /// </summary>
        /// <param name="proID">product ID</param>
        /// <param name="strURL">current page url</param>
        /// <returns>if add cart successfully, return to strURL.</returns>
        public ActionResult AddCart(int proID, string strURL)
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
            
            Product sp = _db.Products.SingleOrDefault(n => n.proID == proID);
            if (sp == null)
            {
                //Trang duong dan khong hop le
                Response.StatusCode = 404;
                return null;
            }
            //Lay gio hang
            List<Cart> lstCart = GetCart();
            // truong hop 1, neu san pham da ton tai trong gio hang
            Cart spCheck = lstCart.SingleOrDefault(n => n.proID == proID);
            if (spCheck != null)
            {
                //Kiem tra so luong ton truoc khi cho khach hang mua
                if (sp.quantity < spCheck.cartQuantity+1)
                {
                    sp.quantity = spCheck.cartQuantity;
                    TempData["outQuan"] = "We are sorry, " + spCheck.proName + " has only " + spCheck.cartQuantity + " left. Please choose other item.";
                    return RedirectToAction("Product","Home");
                }
                spCheck.cartQuantity++;
                spCheck.cartPrice = spCheck.cartQuantity * spCheck.proPrice;
                return Redirect(strURL);
            }
            Cart itemGH = new Cart(proID);
            if (sp.quantity < itemGH.cartQuantity+1)
            {
                sp.quantity = itemGH.cartQuantity;
                TempData["outQuan"] = "We are sorry, " + sp.proName + " has only " + itemGH.cartQuantity + " left. Please choose other item.";
                return RedirectToAction("Product", "Home");
            }

            lstCart.Add(itemGH);
            return Redirect(strURL);

        }




        /// <summary>
        /// Calculate the total quantity
        /// </summary>
        /// <returns>return to cartQuantity</returns>
        //Tinh ton so luong
        public double TotalQuantity()
        {
            //Lay gio hang
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart == null)
            {
                return 0;
            }
            return lstCart.Sum(n => n.cartQuantity);
        }



        /// <summary>
        /// Calculate the total price
        /// </summary>
        /// <returns>return to cartPrice</returns>
        //Tinh tong so tien
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
        /// Action used to create a partial view for cart
        /// </summary>
        /// <returns> a partial view</returns>
        public ActionResult CartPatial()
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
            if (TotalQuantity() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TotalQuantity().ToString("#,##");
            ViewBag.TongTien = TotalPrice().ToString("#,##");
            return PartialView();
        }



        /// <summary>
        /// Action used to return a view based on suitable role session
        /// </summary>
        /// <returns>return lstCart view</returns>
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
            List<Cart> lstCart = GetCart();
            ViewBag.total = TotalPrice().ToString("#,##");
            return View(lstCart);
        }


        /// <summary>
        /// Action used to update cart
        /// </summary>
        /// <param name="proID">product id</param>
        /// <returns>if successfully, return to spCheck view</returns>
        [HttpGet]
        public ActionResult UpdateCart(int proID)
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
            //Kiem tra san pham co ton tai trong CSDL hay ko
            Product sp = _db.Products.SingleOrDefault(n => n.proID == proID);
            if (sp == null)
            {
                //Duong dan ko hop le
                Response.StatusCode = 404;
                return null;
            }
            //Lay list gio hang tu session
            List<Cart> lstCart = GetCart();
            //Kiem tra xem san pham do co ton tai trong gio hang hay ko
            Cart spCheck = lstCart.SingleOrDefault(n => n.proID == proID);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Lay list gio hang tu session
            ViewBag.GioHang = lstCart;
            //Neu ton tai
            return View(spCheck);
        }



        /// <summary>
        /// Action used to update items in cart
        /// </summary>
        /// <param name="itemGH"></param>
        /// <returns>Return Index</returns>
        [HttpPost]
        public ActionResult Update(Cart itemGH)
        {
            //Kiem tra so luong ton
            Product spCheck = _db.Products.Single(n => n.proID == itemGH.proID);
            if (spCheck.quantity < itemGH.cartQuantity)
            {
                TempData["outStock"] = "Please enter quantiy of " + spCheck.proName + " less than " + (spCheck.quantity+1);
                return RedirectToAction("Index");
            }
            if (itemGH.cartQuantity < 1)
            {
                TempData["errorQuan"] = "Quantity must greater than 0";
                return RedirectToAction("Index");
            }
            //Cap nhat so luong trong session gio hang
            //Step 1: lay list<giohang> tu session
            List<Cart> lstGH = GetCart();
            //Step 2: lay sp can cap nhat trong list<gio hang>
            Cart itemUpdate = lstGH.Find(n => n.proID == itemGH.proID);            
            //Step 3: Tien hanh cap nhat lai so luong va gia tien
            itemUpdate.cartQuantity = itemGH.cartQuantity;
            itemUpdate.cartPrice = itemUpdate.cartQuantity * itemUpdate.proPrice;
            return RedirectToAction("Index");

        }


        /// <summary>
        /// Action to delete a cart by product id
        /// </summary>
        /// <param name="proID"></param>
        /// <returns>if delete successfully, return to Index view</returns>
        public ActionResult DeleteCart(int proID)
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
            //Kiem tra san pham co ton tai trong CSDL hay ko
            Product sp = _db.Products.SingleOrDefault(n => n.proID == proID);
            if (sp == null)
            {
                //Duong dan ko hop le
                Response.StatusCode = 404;
                return null;
            }
            //Lay list gio hang tu session
            List<Cart> lstCart = GetCart();
            //Kiem tra xem san pham do co ton tai trong gio hang hay ko
            Cart spCheck = lstCart.SingleOrDefault(n => n.proID == proID);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }

            //Xoa item trong gio hang
            lstCart.Remove(spCheck);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Action used to return a view of bill index
        /// </summary>
        /// <returns>a view</returns>
        [HttpPost]
        public ActionResult Order()
        {            
            return RedirectToAction("Index","Bill");
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