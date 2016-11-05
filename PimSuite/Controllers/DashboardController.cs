using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PimSuite.Models;

namespace PimSuite.Controllers
{
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
            ViewBag.TotalUser = _db.Users.Count();
            ViewBag.TotalRole = _db.Roles.Count();
            var users = _db.Users.Include("Roles");

            return View(users.ToList());
        }
    }
}