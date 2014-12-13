using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Logging;

namespace MvcApplicationWithLogging.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var logger = MvcApplication.Container.Resolve<ILogger>();

            for (int i = 0; i < 10000; i++)
            {
                logger.Error("My error");    
            }
            


            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
