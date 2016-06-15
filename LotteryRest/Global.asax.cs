using LotteryRest.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LotteryRest
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static Random randomSeed = new Random();
        protected void Application_Start()
        {
            //GlobalConfiguration.Configuration.MessageHandlers.Add(new CorsHandler());
            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy= IncludeErrorDetailPolicy.Always;
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
