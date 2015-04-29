using System;
using System.Web.Mvc;

namespace TaxOrg.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            var error = (Exception)Session["errorObject"];
            return View(error);
        }
    }
}