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
    public class SupplierController : Controller
    {
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
            return View();
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

        // GET: Supplier/Edit/5
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
            int tempID = Convert.ToInt32(Session["ID"]);
            var listTemp = _db.BillDetails.Where(x => x.Products.Supplier.supID == tempID).ToList();
            return View(listTemp.GroupBy(x => x.bID).Select(g => g.First()).Where(y => y.Bill.uID == id).ToList());
        }
    }
}
