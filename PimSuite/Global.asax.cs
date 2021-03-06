﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PimSuite.Models;

namespace PimSuite
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            CleanUpConnectionData();
            GlobalFilters.Filters.Add(new AuthorizeAttribute());
            AreaRegistration.RegisterAllAreas();
//            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private void CleanUpConnectionData()
        {
            var db = new PimSuiteDatabaseEntities();
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE [Connections]");
        }
    }
}
