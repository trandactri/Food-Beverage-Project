using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
    /// Controller for Admin
    /// </summary>
    public class AdminController : Controller
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
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (Session["Role"].ToString() != "admin")
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
            return View();
        }

        /// <summary>
        /// Action used to return a view with a list of users based on role session
        /// </summary>
        /// <returns> User Management view with a list of users </returns>
        public ActionResult UserManagement()
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (Session["Role"].ToString() != "admin")
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
            return View(_db.Users.ToList());
        }

        /// <summary>
        /// Action used to return a view with a list of suppliers based on role session
        /// </summary>
        /// <returns> Supplier Management view with a list of suppliers </returns>
        public ActionResult SupplierManagement()
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (Session["Role"].ToString() != "admin")
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
            return View(_db.Suppliers.ToList());
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
        /// <param name="adUsername"> Admin username after login </param>
        /// <param name="adPwd"> Admin Password after login </param>
        /// <returns>If successfully login, assign to sessions, return to Index page, else ask for trying again</returns>
        [HttpPost]
        public ActionResult Login(string adUsername, string adPwd)
        {
            if (ModelState.IsValid)
            {
                var f_password = HomeController.GetMD5(adPwd);
                var data = _db.Admins.Where(s => s.adUsername.Equals(adUsername) && s.adPwd.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["FullName"] = data.FirstOrDefault().adFirstname + " " + data.FirstOrDefault().adLastname;
                    Session["ID"] = data.FirstOrDefault().adID;
                    Session["Role"] = "admin";
                    Session.Timeout = 60;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Incorrect username or password. Try again";
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
        /// Action used to return admin profile view based on adID
        /// </summary>
        /// <param name="id">Admin id after clicking profile button</param>
        /// <returns>Return AdminProfile view based on adid if session role is admin, else return login page</returns>
        public ActionResult AdminProfile(int? id)
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() != "admin")
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
            id = (int?)Session["ID"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = _db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        /// <summary>
        /// Action used to return edit profile view based on adID
        /// </summary>
        /// <param name="id">Admin id after clicking profile button</param>
        /// <returns>Return EditProfile view based on adid if session role is admin, else return login page</returns>
        public ActionResult EditProfile(int? id)
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() != "admin")
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
            id = (int?)Session["ID"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = _db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        /// <summary>
        /// After editting an admin profile, send values back to server for processing
        /// </summary>
        /// <param name="admin">Admin object after editting</param>
        /// <returns>If successfully edit, return to AdminProfile view, else ask for trying again</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile([Bind(Include = "adID,adUsername,adPwd,adFirstname,adLastname,adPhone,adEmail,ConfirmPassword")] Admin admin)
        {

            if (ModelState.IsValid)
            {
                var check = _db.Admins.FirstOrDefault(s => s.adEmail == admin.adEmail && s.adID != admin.adID);
                if (check == null)
                {
                    _db.Entry(admin).State = EntityState.Modified;
                    _db.SaveChanges();
                    Session["Fullname"] = admin.adFirstname + " " + admin.adLastname;
                    return RedirectToAction("AdminProfile", "Admin");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View(admin);
                }
            }
            return View(admin);
        }

        /// <summary>
        /// Action used to return edit password view based on adID
        /// </summary>
        /// <param name="id">adID after click edit password button</param>
        /// <returns>Return EditPassword view based on adid if session role is admin, else return login page</returns>
        public ActionResult EditPassword(int? id)
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() != "admin")
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
            id = (int?)Session["ID"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = _db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        /// <summary>
        /// After editting a password, send values back to server for processing
        /// </summary>
        /// <param name="admin">Admin object after editting</param>
        /// <returns>If successfully edit, return to admin login view to login again, else ask for trying again</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassword([Bind(Include = "adID,adUsername,adPwd,adFirstname,adLastname,adPhone,adEmail,ConfirmPassword")] Admin admin)
        {

            if (ModelState.IsValid)
            {
                admin.adPwd = HomeController.GetMD5(admin.adPwd);
                admin.ConfirmPassword = admin.adPwd;
                _db.Entry(admin).State = EntityState.Modified;
                _db.SaveChanges();
                TempData["success"] = "Successfully change password. Please login again";
                return RedirectToAction("Login", "Admin");
            }
            return View(admin);
        }


        /// <summary>
        /// Action used to return edit user view based on uID
        /// </summary>
        /// <param name="id">uID after click edit user button</param>
        /// <returns>Return EditUser view based on uid if session role is admin, else return login page</returns>
        public ActionResult EditUser(int? id)
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (Session["Role"].ToString() != "admin")
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        /// <summary>
        /// After editting a user profile, send values back to server for processing
        /// </summary>
        /// <param name="user">User object after editting</param>
        /// <returns>If successfully edit, return UserManagement view, else ask to try again</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser([Bind(Include = "uID,uUsername,uPwd,uFirstname,uLastname,uPhone,uAddress,uBirthday,uEmail,uGender,uCreditCard,ConfirmPassword")] User user)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("UserManagement", "Admin");
            }
            return View(user);
        }

        /// <summary>
        /// Action used to return delete user view based on uID
        /// </summary>
        /// <param name="id">uID after clicking delete user button</param>
        /// <returns>Return DeleteUser view based on uID if session role is admin, else return login page</returns>
        public ActionResult DeleteUser(int? id)
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (Session["Role"].ToString() != "admin")
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        /// <summary>
        /// Action used to return delete user view based on supID
        /// </summary>
        /// <param name="id">uID after clicking delete user button</param>
        /// <returns>Return DeleteUser view based on supID if session role is admin, else return login page</returns>
        public ActionResult DeleteSupplier(int? id)
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (Session["Role"].ToString() != "admin")
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
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
        /// After agree on deleting user, send values back to server for processing, set isActive value to false
        /// </summary>
        /// <param name="id">uID after clicking delete user button</param>
        /// <returns>If successfully save changes, return UserManagement view</returns>
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserConfirmed(User obj)
        {

            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (Session["Role"].ToString() != "admin")
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
            if (obj == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obj.ConfirmPassword = obj.uPwd;
            obj.isActive = false;
            _db.Entry(obj).State = EntityState.Modified;
            this._db.SaveChanges();
            return RedirectToAction("UserManagement", "Admin");
        }


        /// <summary>
        /// Action used to return delete supplier view based on supID
        /// </summary>
        /// <param name="id">supID after clicking delete supplier button</param>
        /// <returns>Return DeleteSupplier view based on supID if session role is admin, else return login page</returns>
        public ActionResult DetailsSupplier(int? id)
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (Session["Role"].ToString() != "admin")
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = _db.Suppliers.Find(id);
            if (supplier == null && !supplier.supStatus)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        /// <summary>
        /// After agree on deleting supplier, send values back to server for processing, set supStatus value to false
        /// </summary>
        /// <param name="id">supID after clicking delete supplier button</param>
        /// <returns>If successfully save changes, return SupplierManagement view</returns>
        [HttpPost, ActionName("DeleteSupplier")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSupplierConfirmed(Supplier obj)
        {

            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (Session["Role"].ToString() != "admin")
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
            if (obj == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obj.ConfirmPassword = obj.supPwd;
            obj.supStatus = false;
            _db.Entry(obj).State = EntityState.Modified;
            this._db.SaveChanges();
            return RedirectToAction("SupplierManagement", "Admin");

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
