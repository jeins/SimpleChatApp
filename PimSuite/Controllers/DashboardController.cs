using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PimSuite.Models;

namespace PimSuite.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private PimSuiteDatabaseEntities _db;
        private FormsAuthenticationTicket _authTicket;

        public DashboardController()
        {
            _db = new PimSuiteDatabaseEntities();
        }

        // GET: Dashboard/Index
        public ActionResult Index()
        {
            ViewBag.TotalUser = _db.Users.Count();
            ViewBag.TotalRole = _db.Roles.Count();
            ViewBag.UserName = this.GetUserName();

            var users = _db.Users.Include("Roles");

            return View(users.ToList());
        }

        private string GetUserName()
        {

            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            _authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            var user = _db.Users.FirstOrDefault(u => u.Email == _authTicket.Name);
            
            return user.FirstName + " " + user.LastName;
        }
    }
}