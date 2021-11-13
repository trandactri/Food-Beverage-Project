using LoginandR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace LoginandR.Controllers
{
    /// <summary>
    /// Controller for home
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Variable used for activities on DB_Entities
        /// </summary>
        private DB_Entities _db = new DB_Entities();

        /// <summary>
        /// Action used to return a view
        /// </summary>
        /// <returns> Index view </returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Action used to return a view
        /// </summary>
        /// <param name="id">product id</param>
        /// <returns>a view of product match id</returns>
        public ActionResult Details(int? id)
        {
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
        /// Action used to return a view
        /// </summary>
        /// <returns> Contact us view </returns>
        public ActionResult Contact()
        {
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
        /// After signing up a new user account, send values back to server for processing
        /// </summary>
        /// <param name="_user">User object after signing up</param>
        /// <returns>If successfully sign up, return to User login page, else ask for trying again</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _user)
        {
            //If values are similar to admin account, create a warning viewbag and return to sign up view
            if (_user.uUsername == "admin" || _user.uEmail == "admin@gmail.com")
            {
                ViewBag.dupAdmin = "Cannot use this information";
                return View();
            }
            if (ModelState.IsValid)
            {
                var checkEmail = _db.Users.FirstOrDefault(s => s.uEmail == _user.uEmail);
                var checkUS = _db.Users.FirstOrDefault(s => s.uUsername == _user.uUsername);
                if (checkEmail == null)
                {
                    if (checkUS == null)
                    {
                        _user.uPwd = GetMD5(_user.uPwd);
                        _db.Configuration.ValidateOnSaveEnabled = false;
                        _db.Users.Add(_user);
                        _db.SaveChanges();
                        return RedirectToAction("Login", "Home");
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
        /// Delete all sessions and return to user login view
        /// </summary>
        /// <returns>User login view</returns>
        public ActionResult Login()
        {
            Session.Clear();
            return View();
        }


        /// <summary>
        /// After login, send values back to server to processing
        /// </summary>
        /// <param name="uUsername">User username after login</param>
        /// <param name="uPwd">User Password after login</param>
        /// <returns>If successfully login, assign to sessions, return to Index home page, else ask for trying again</returns>
        [HttpPost]
        public ActionResult Login(string uUsername, string uPwd)
        {
            if (ModelState.IsValid)
            {


                var f_password = GetMD5(uPwd);
                var data = _db.Users.Where(s => s.uUsername.Equals(uUsername) && s.uPwd.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["FullName"] = data.FirstOrDefault().uFirstname;
                    Session["Phone"] = data.FirstOrDefault().uPhone;
                    Session["Address"] = data.FirstOrDefault().uAddress;
                    Session["ID"] = data.FirstOrDefault().uID;
                    Session["CardNum"] = data.FirstOrDefault().uCreditCard;
                    Session["Role"] = "user";
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
        /// Action used to hash a string to a new one with md5 technique
        /// </summary>
        /// <param name="str">A string user enter</param>
        /// <returns> A md5 hashing string </returns>
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        /// <summary>
        /// Action used to return a list of product view
        /// </summary>
        /// <param name="page">page number</param>
        /// <returns>a list of product view</returns>
        public ActionResult Product(int? page)
        {            
            var data = (from s in _db.Products where s.proStatus && s.Supplier.supStatus select s);
            if (page > 0)
            {
                page = page;
            }
            else
            {
                page = 1; // set page default 1
            }
            int limit = 6; //display show 3 product 
            int start = (int)(page - 1) * limit;
            int totalProduct = data.Count();
            ViewBag.totalProduct = totalProduct;
            ViewBag.pageCurrent = page;
            float numberPage = (float)totalProduct / limit;
            ViewBag.numberPage = (int)Math.Ceiling(numberPage);
            var dataProduct = data.OrderByDescending(s => s.proID).Where(s => s.proStatus && s.Supplier.supStatus).Skip(start).Take(limit);          
            if (page > ViewBag.numberPage)
            {
                return HttpNotFound();
            }
            return View(dataProduct.ToList());
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