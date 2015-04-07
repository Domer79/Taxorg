using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemTools;

namespace TaxOrg.Controllers
{
    public class LogonController : Controller
    {
        // GET: Logon
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string login, string password)
        {
            if (ApplicationCustomizer.Security.Sign(login, password))
            {
                ApplicationCustomizer.Security.CreateCookie(login);
                RedirectToAction("Index", "Org");
            }
            return View();
        }
    }
}