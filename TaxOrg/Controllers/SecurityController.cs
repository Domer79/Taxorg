using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using SystemTools.Extensions;
using IntellISenseSecurity;
using WebSecurity.CmdRun;
using WebSecurity.IntellISense;

namespace TaxOrg.Controllers
{
    public class SecurityController : Controller
    {
        // GET: Security
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users()
        {
            return View();
        }

        public ActionResult Controllers()
        {
            return View();
        }

        public ActionResult CmdRun(string command)
        {
            try
            {
                var termDispatcher = new CommandTermDispatcher<CommandTermMain>(command);
                var runDispatcher = new CommandRunDispatcher(new SecurityCommandRun());
                runDispatcher.Run(termDispatcher.Stack);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                const int messageStringCount = 500;
                e.SaveError();
                var errorMessage = e.GetErrorMessage();
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, errorMessage.Length > messageStringCount ? errorMessage.Substring(0, messageStringCount) : e.Message);
            }
        }

        public ActionResult Intellisense(string term)
        {
            var result = (IEnumerable<string>)new CommandTermDispatcher<CommandTermMain>(term);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}