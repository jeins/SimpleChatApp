using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PimSuite.Models;

namespace PimSuite.Controllers
{
    public class UserController : Controller
    {

        //GET User/Login
        public ActionResult Login(UserMeta model)
        {
            if (model.Email != null && model.Password != null)
            {
                return RedirectToRoute(new {controller = "Dashboard", action = "Index"});
            }

            return View();
        }

        //GET User/Register
        public ActionResult Register(UserMeta model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Email != null && model.FirstName != null && model.LastName != null && model.Password != null)
            {
                return RedirectToRoute(new { controller = "Dashboard", action = "Index" });
            }
            return View();
        }
    }
}