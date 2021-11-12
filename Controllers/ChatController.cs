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
    public class ChatController : Controller
    {
        /// <summary>
        /// Action used to return a view of user chat
        /// </summary>
        /// <returns>a view</returns>
        public ActionResult ChatUser()
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        /// <summary>
        /// Action used to return a view of admin chat
        /// </summary>
        /// <returns>a view</returns>
        public ActionResult ChatAdmin()
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
    }
}