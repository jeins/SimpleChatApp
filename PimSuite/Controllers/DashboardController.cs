using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using PimSuite.Models;

namespace PimSuite.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private PimSuiteDatabaseEntities _db;

        public DashboardController()
        {
            _db = new PimSuiteDatabaseEntities();
        }

        // GET: Dashboard/Index
        public ActionResult Index()
        {
            var user = GetUserFromCookie();

            ViewBag.TotalUser = _db.Users.Count();
            ViewBag.TotalRole = _db.Roles.Count();
            ViewBag.UserName = user.FirstName + " " + user.LastName;

            var users = _db.Users.Include("Roles");

            return View(users.ToList());
        }

        // GET: Dashboard/Chat
        public ActionResult Chat()
        {
            var user = GetUserFromCookie();

            ViewBag.FullName = user.FirstName + " " + user.LastName;
            ViewBag.UserId = user.UserId;

            return View();
        }

        private Users GetUserFromCookie()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            var serializer = new JavaScriptSerializer();
            var user = (Users) serializer.Deserialize(authTicket.UserData, typeof(Users));

            return user;
        }
    }
}