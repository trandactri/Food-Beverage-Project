using LoginandR.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LoginandR.Controllers
{
    public class UserController : Controller
    {
        private DB_Entities _db = new DB_Entities();

        /// <summary>
        /// Action used to return user index view based on uID
        /// </summary>
        /// <param name="id">uID after signing in</param>
        /// <returns>Return to user index view based on uID if session role is user, else redirect to home login page</returns>
        public ActionResult Index(int? id)
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() != "user")
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            id = (int?)Session["ID"];

            if (id == null)
            {
                return RedirectToAction("Login", "Home");
            }
            User user = _db.Users.Find(id); // Assign to user object if find user with suitable uID, else return notfound view
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        /// <summary>
        /// Action used to return an edit user view based on uID
        /// </summary>
        /// <param name="id">uID after clicking edit button</param>
        /// <returns>Return user edit view based on uID if session role is user, else home login page</returns>
        public ActionResult Edit(int? id)
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() != "user")
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            id = (int?)Session["ID"];

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
        /// After editting an user profile, send values back to server for processing
        /// </summary>
        /// <param name="user">User object after editting</param>
        /// <returns>If successfully edit, return user index view, else ask for trying again</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "uID,uUsername,uPwd,uFirstname,uLastname,uPhone,uAddress,uBirthday,uEmail,uGender,uCreditCard,ConfirmPassword")] User user)
        {
            if (user.uEmail == "admin@gmail.com")
            {
                ViewBag.dupAdmin = "Cannot use this information";
                return View(user);
            }
            if (ModelState.IsValid)
            {
                var check = _db.Users.FirstOrDefault(s => s.uEmail == user.uEmail && s.uID != user.uID);
                if (check == null)
                {
                    _db.Entry(user).State = EntityState.Modified;
                    _db.SaveChanges();
                    Session["Fullname"] = user.uFirstname + " " + user.uLastname;
                    Session["Phone"] = user.uPhone;
                    Session["CardNum"] = user.uCreditCard;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View(user);
                }
            }
            return View(user);
        }

        /// <summary>
        /// Action used to return edit password view based on uID
        /// </summary>
        /// <param name="id">uID after click edit password button</param>
        /// <returns>Return EditPassword view based on uid if session role is user, else return login page</returns>
        public ActionResult EditPassword(int? id)
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() != "user")
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            id = (int?)Session["ID"];

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
        /// After editting a password, send values back to server for processing
        /// </summary>
        /// <param name="user">User object after editting</param>
        /// <returns>If successfully edit, return to user login view to login again, else ask for trying again</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassword([Bind(Include = "uID,uUsername,uPwd,uFirstname,uLastname,uPhone,uAddress,uBirthday,uEmail,uGender,uCreditCard,ConfirmPassword")] User user)
        {

            if (ModelState.IsValid)
            {
                user.uPwd = HomeController.GetMD5(user.uPwd);
                user.ConfirmPassword = user.uPwd;
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
                TempData["success"] = "Successfully change password. Please login again";
                return RedirectToAction("Login", "Home");
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult Orders()
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() != "user")
                {
                    return RedirectToAction("Login", "Home");
                }                
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

            int tempID = Convert.ToInt32(Session["ID"]);
            return View(_db.Bills.Where(x => x.uID == tempID).ToList());
        }
    }
}