using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using LoginandR.Models;

namespace LoginandR.Controllers
{
    /// <summary>
    /// Controller for supplier
    /// </summary>
    public class SupplierController : Controller
    {
        /// <summary>
        /// Variable used for activities on DB_Entities
        /// </summary>
        private DB_Entities _db = new DB_Entities();

        /// <summary>
        /// Action used to return a view based on suitable role session
        /// </summary>
        /// <returns> Index view </returns>
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
            int tempID = Convert.ToInt32(Session["ID"]);
            return View(_db.Feedbacks.Where(x => x.supID == tempID).ToList());
        }

        /// <summary>
        /// Action used to return a view
        /// </summary>
        /// <returns> Register view </returns>
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// After signing up a new supplier account, send values back to server for processing
        /// </summary>
        /// <param name="_supplier">Supplier object after signing up</param>
        /// <returns>If successfully sign up, return to Suppier login page, else ask for trying again</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Supplier _supplier)
        {
            if (ModelState.IsValid)
            {
                AddImage(_supplier);
                var checkEmail = _db.Suppliers.FirstOrDefault(s => s.supEmail == _supplier.supEmail);
                var checkUS = _db.Suppliers.FirstOrDefault(s => s.supUsername == _supplier.supUsername);
                if (checkEmail == null)
                {
                    if (checkUS == null)
                    {
                        _supplier.supPwd = HomeController.GetMD5(_supplier.supPwd);
                        _db.Configuration.ValidateOnSaveEnabled = false;
                        _db.Suppliers.Add(_supplier);
                        _db.SaveChanges();
                        return RedirectToAction("Login", "Supplier");
                    }
                    else
                    {
                        ViewBag.errorUS = "Username already exists";
                        return View();
                    }
                }
                else
                {
                    ViewBag.errorEmail = "Email already exists";
                    return View();
                }
            }
            return View();


        }

        /// <summary>
        /// Action used to delete all sessions and return a view
        /// </summary>
        /// <returns> Login view  </returns>
        public ActionResult Login()
        {
            Session.Clear();
            return View();
        }

        /// <summary>
        /// After login, send values back to server to processing
        /// </summary>
        /// <param name="supUsername">Supplier username after login</param>
        /// <param name="supPwd">Supplier Password after login</param>
        /// <returns>If successfully login, assign to sessions, return to supplier index page, else ask for trying again</returns>
        [HttpPost]
        public ActionResult Login(string supUsername, string supPwd)
        {
            if (ModelState.IsValid)
            {


                var f_password = HomeController.GetMD5(supPwd);
                var data = _db.Suppliers.Where(s => s.supUsername.Equals(supUsername) && s.supPwd.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["FullName"] = data.FirstOrDefault().supName;
                    Session["ID"] = data.FirstOrDefault().supID;
                    Session["Role"] = "supplier";
                    Session["SupImg"] = data.FirstOrDefault().supImg;
                    Session.Timeout = 60;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Login failed. Incorrect username or password.";
                    return View();
                }
            }
            return View();
        }


        /// <summary>
        /// Action used to delete all sessions and return a view
        /// </summary>
        /// <returns> Login view  </returns>
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Action used to return supplier profile view based on supID
        /// </summary>
        /// <param name="id">Supplier id after clicking profile button</param>
        /// <returns>Return SupplierProfile view based on supid if session role is supplier, else return login page</returns>
        public ActionResult SupplierProfile(int? id)
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() != "supplier")
                {
                    return RedirectToAction("Login", "Supplier");
                }
            }
            id = (int?)Session["ID"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = _db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        /// <summary>
        /// Action used to return edit profile view based on supID
        /// </summary>
        /// <param name="id">Supplier id after clicking profile button</param>
        /// <returns>Return Edit view based on supid if session role is supplier, else return login page</returns>
        public ActionResult Edit(int? id)
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() != "supplier")
                {
                    return RedirectToAction("Login", "Supplier");
                }
            }
            id = (int?)Session["ID"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = _db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        /// <summary>
        /// After editting an supplier profile, send values back to server for processing
        /// </summary>
        /// <param name="supplier">Supplier object after editting</param>
        /// <returns>If successfully edit, return to SupplierProfile view, else ask for trying again</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "supID,supUsername,supPwd,supName,supPhone,supAddress,supEmail,supDescription,supImg,ConfirmPassword")] Supplier supplier)
        {
            if (supplier.supEmail == "admin@gmail.com")
            {
                ViewBag.dupAdmin = "Cannot use this information";
                return View(supplier);
            }
            if (ModelState.IsValid)
            {
                var check = _db.Suppliers.FirstOrDefault(s => s.supEmail == supplier.supEmail && s.supID != supplier.supID);
                if (check == null)
                {
                    _db.Entry(supplier).State = EntityState.Modified;
                    _db.SaveChanges();
                    Session["Fullname"] = supplier.supName;
                    return RedirectToAction("SupplierProfile", "Supplier");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View(supplier);
                }
            }
            return View(supplier);
        }

        /// <summary>
        /// Action used to return edit password view based on supID
        /// </summary>
        /// <param name="id">supID after click edit password button</param>
        /// <returns>Return EditPassword view based on supID if session role is supplier, else return login page</returns>
        public ActionResult EditPassword(int? id)
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() != "supplier")
                {
                    return RedirectToAction("Login", "Supplier");
                }
            }
            id = (int?)Session["ID"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = _db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        /// <summary>
        /// After editting a password, send values back to server for processing
        /// </summary>
        /// <param name="supplier">Supplier object after editting</param>
        /// <returns>If successfully edit, return to supplier login view to login again, else ask for trying again</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassword([Bind(Include = "supID,supUsername,supPwd,supName,supPhone,supAddress,supEmail,supDescription,supImg,ConfirmPassword")] Supplier supplier)
        {

            if (ModelState.IsValid)
            {
                supplier.supPwd = HomeController.GetMD5(supplier.supPwd);
                supplier.ConfirmPassword = supplier.supPwd;
                _db.Entry(supplier).State = EntityState.Modified;
                _db.SaveChanges();
                TempData["success"] = "Successfully change password. Please login again";
                return RedirectToAction("Login", "Supplier");
            }
            return View(supplier);
        }


        /// <summary>
        /// Action used to return a view for changing avatar
        /// </summary>
        /// <param name="id">supplier id</param>
        /// <returns>a view</returns>
        public ActionResult ChangeAvatar(int? id)
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() != "supplier")
                {
                    return RedirectToAction("Login", "Supplier");
                }
            }
            id = (int?)Session["ID"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = _db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        /// <summary>
        /// After change avatar, send datas back to this action for processing
        /// </summary>
        /// <param name="supplier">supplier object</param>
        /// <returns>save changes after changing, return to supplier profile action</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeAvatar(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                AddImage(supplier);
                Session["SupImg"] = supplier.supImg;
                _db.Entry(supplier).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("SupplierProfile", "Supplier");
            }
            return View(supplier);
        }

        /// <summary>
        /// Action used to return a view with a list of feedbacks
        /// </summary>
        /// <returns>Feedback Management view with a list of feedbacks</returns>
        public ActionResult FeedbackManagement()
        {
            return RedirectToAction("Index", "Feedback");
        }

        /// <summary>
        /// Function used to add images to pointed folder with different path and file name
        /// </summary>
        /// <param name="_supplier">Supplier object</param>
        public void AddImage(Supplier _supplier)
        {
            string fileName = Path.GetFileNameWithoutExtension(_supplier.ImageFile.FileName);
            string extension = Path.GetExtension(_supplier.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension; //change filename with different format (EX: abc215011996.jpg)
            _supplier.supImg = "~/Image/" + fileName; // assign supImg image to pointed folder path
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName); //combine folder path
            _supplier.ImageFile.SaveAs(fileName); // save file as above path
        }

        /// <summary>
        /// Action used to return a view of product list match tempid
        /// </summary>
        /// <returns>a view</returns>
        public ActionResult ProductManagement()
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
            int tempID = Convert.ToInt32(Session["ID"]);
            return View(_db.Products.Where(x => x.supID == tempID).ToList());
        }

        /// <summary>
        /// Action used to return a view of user bill list
        /// </summary>
        /// <returns>a view</returns>
        public ActionResult UserBill()
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
            int tempID = Convert.ToInt32(Session["ID"]);
            var listTemp = _db.BillDetails.Where(x => x.Products.Supplier.supID == tempID).ToList();
            return View(listTemp.GroupBy(x => x.Bill.uID).Select(g => g.First()).ToList());
        }

        /// <summary>
        /// Action used to return a view of bill management
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>a view</returns>
        public ActionResult BillManagement(int? id)
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
            var checkID = _db.Users.Find(id);
            if (checkID == null)
            {
                return HttpNotFound();
            }
            int tempID = Convert.ToInt32(Session["ID"]);
            var listTemp = _db.BillDetails.Where(x => x.Products.Supplier.supID == tempID).ToList();
            return View(listTemp.GroupBy(x => x.bID).Select(g => g.First()).Where(y => y.Bill.uID == id).ToList());
        }

        /// <summary>
        /// Action used to return bill detail based on bill detail id
        /// </summary>
        /// <param name="id">bill detail id</param>
        /// <returns>bill detail</returns>
        public ActionResult BillDetail(int? id)
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
            int tempID = Convert.ToInt32(Session["ID"]);
            IEnumerable<BillDetail> billDe = _db.BillDetails.Where(x => x.bID == id && x.Products.supID == tempID).ToList();
            if (billDe == null)
            {
                return HttpNotFound();
            }
            return View(billDe);
        }

        /// <summary>
        /// Action used to return a create view
        /// </summary>
        /// <returns>a view</returns>
        public ActionResult Create()
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
            var list = new List<SelectListItem>
            {
                 new SelectListItem{ Text="Food", Value = "1", Selected = true },
                 new SelectListItem{ Text="Beverage", Value = "2" },
            };
            ViewData["foorBarList"] = list;
            return View();
        }

        /// <summary>
        /// After create product, send values back to this action to process
        /// </summary>
        /// <param name="product">product object</param>
        /// <returns>If successfully add return productmanagement page, else return view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (product == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                this._db.Products.Add(product);
                this._db.SaveChanges();
                return RedirectToAction("ProductManagement", "Supplier");
            }

            return View(product);
        }

        /// <summary>
        /// Action used to return an update product view
        /// </summary>
        /// <param name="id">product id</param>
        /// <returns>a view</returns>
        public ActionResult UpdateProduct(int? id)
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
            Product product = _db.Products.Find(id);
            TempData["img"] = product.proImg;
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        /// <summary>
        /// After update product, send values back to this action to process
        /// </summary>
        /// <param name="product">product object</param>
        /// <returns>If successfully update product return product management page, else return view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProduct(Product product)
        {
            if (product == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                if (product.proImg == null || product.proImg == "")
                {
                    product.proImg = TempData["img"].ToString();
                }
                this._db.Entry(product).State = EntityState.Modified;
                this._db.SaveChanges();
                return RedirectToAction("ProductManagement", "Supplier");
            }
            return View(product);
        }

        /// <summary>
        /// Action used to return a delete product view
        /// </summary>
        /// <param name="id">product id</param>
        /// <returns>a view</returns>
        public ActionResult DeleteProduct(int? id)
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
            Product product = _db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        /// <summary>
        /// After confirm delete, find id match product id, change pro status to false 
        /// </summary>
        /// <param name="id">product id</param>
        /// <returns>product management page</returns>
        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProductConfirmed(int? id)
        {
            Product product = _db.Products.Find(id);
            product.proStatus = false;
            /*_db.Products.Remove(product);*/
            this._db.SaveChanges();
            return RedirectToAction("ProductManagement", "Supplier");
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
