using LoginandR.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LoginandR.Controllers
{
    /// <summary>
    /// Controller for user
    /// </summary>
    public class UserController : Controller
    {
        /// <summary>
        /// Variable used for activities on DB_Entities
        /// </summary>
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
            else
            {
                return RedirectToAction("Login", "Home");
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
            else
            {
                return RedirectToAction("Login", "Home");
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
            else
            {
                return RedirectToAction("Login", "Home");
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

        /// <summary>
        /// Action used to return a view of orders of this user based on uid
        /// </summary>
        /// <returns>a view</returns>
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

        /// <summary>
        /// Action used to return an order detail of this user
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>a view</returns>
        [HttpGet]
        public ActionResult OrderDetail(int? id)
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var check = _db.Bills.Find(id);
            if (check == null)
            {
                return HttpNotFound();
            }
            IEnumerable<BillDetail> billDe = _db.BillDetails.Where(x => x.bID == id).ToList();
            if (billDe == null)
            {
                return HttpNotFound();
            }
            return View(billDe);
        }


        /// <summary>
        /// Action used to return a view of forgot password
        /// </summary>
        /// <returns>a view</returns>
        public ActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// After enter values to the form, get datas back to this action to process,
        /// then send to existed email in db a reset pwd link, if not return error message
        /// </summary>
        /// <param name="EmailID">email address</param>
        /// <returns></returns>
        // Add another action in our UserController for verifying the email id
        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            //Declare variable
            string resetCode = Guid.NewGuid().ToString();
            var verifyUrl = "/User/ResetPassword/" + resetCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            //Sent content to email user 
            using (var context = new DB_Entities())
            {
                var getUser = (from s in context.Users where s.uEmail == EmailID select s).FirstOrDefault();
                if (getUser != null)
                {
                    getUser.ResetPasswordCode = resetCode;

                    //This line to avoid confirm password not match issue , as we had added a confirm password property 
                    context.Configuration.ValidateOnSaveEnabled = false;
                    context.SaveChanges();

                    var subject = "Password Reset Request";
                    var body = "Hi " + getUser.uFirstname + ", <br/> You recently requested to reset your password for your account. Click the link below to reset it. " +

                         " <br/><br/><a href='" + link + "'>" + link + "</a> <br/><br/>" +
                         "If you did not request a password reset, please ignore this email or reply to let us know.<br/><br/> Thank you";

                    SendEmail(getUser.uEmail, body, subject);

                    ViewBag.Message = "Reset password link has been sent to your email id.";
                }
                // Notification when account email user not found
                else
                {
                    ViewBag.Message = "User doesn't exists.";
                    return View();
                }
            }

            return View();
        }

        /// <summary>
        /// Action used to configure host email address and send to others
        /// </summary>
        /// <param name="emailAddress">email address</param>
        /// <param name="body">body message</param>
        /// <param name="subject">subject message</param>
        //Config Email sent to user
        private void SendEmail(string emailAddress, string body, string subject)
        {
            //function for sending Reset Password email 
            using (MailMessage mm = new MailMessage("tritdce150815@fpt.edu.vn", emailAddress))
            {
                mm.Subject = subject;
                mm.Body = body;

                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                //Email and password of email user guider
                NetworkCredential NetworkCred = new NetworkCredential("tritdce150815@fpt.edu.vn", "13879428");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);

            }
        }

        /// <summary>
        /// Action used to return a view of a specific user reset password 
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>a view</returns>
        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            using (var context = new DB_Entities())
            {
                var user = context.Users.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        /// <summary>
        /// After reset password, send datas back to this action to process
        /// </summary>
        /// <param name="model">reset password model object</param>
        /// <returns>If successfully reset password, return successful message, else return error message, return to home</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (var context = new DB_Entities())
                {
                    var user = context.Users.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        //encrypt password here, we are not doing it
                        user.uPwd = HomeController.GetMD5(model.NewPassword);
                        //make resetpasswordcode empty string now
                        user.ResetPasswordCode = "";
                        //to avoid validation issues, disable it
                        context.Configuration.ValidateOnSaveEnabled = false;
                        context.SaveChanges();
                        message = "New password updated successfully";
                    }
                }
            }
            else
            {
                message = "Something invalid";
            }
            TempData["Message"] = message;
            return RedirectToAction("Login","Home");
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