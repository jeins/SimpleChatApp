using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PimSuite.Models;

namespace PimSuite.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        private PimSuiteDatabaseEntities _db;

        public UserController()
        {
            _db = new PimSuiteDatabaseEntities();
        }

        //GET User/Login
        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.ErrorMessage = "";
            return View();
        }

        [HttpPost]
        public ActionResult Login(Users model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Please Feel All Required Field!";
                return View(model);
            }

            var count = _db.Users.Count(u => u.Email == model.Email && u.Password == model.Password);

            if (count == 0)
            {
                ViewBag.ErrorMessage = "Invalid User!";
                return View();
            }
            else
            {
                string userData = model.FirstName + " " + model.LastName;
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1, model.Email, DateTime.Now, DateTime.Now.AddMinutes(10), false, userData);

                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
//                FormsAuthentication.SetAuthCookie(cookie, false);
                this.Response.Cookies.Add(cookie);
                return RedirectToAction("Index", "Dashboard");
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);

            this.Response.Cookies.Add(cookie);

            return RedirectToAction("Login", "User");
        }
    }
}