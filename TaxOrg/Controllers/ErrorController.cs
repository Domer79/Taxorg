using System;
using System.Web;
using System.Web.Mvc;
using SystemTools.Extensions;

namespace TaxOrg.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            Exception error;
            if (Session["errorObject"] == null)
            {
                var errorCookie = HttpContext.Request.Cookies["exception"];

                if (errorCookie == null || string.IsNullOrEmpty(errorCookie.Value))
                    error = null;
                else
                    error = ObjectHelper.DeserializeFromString<Exception>(errorCookie.Value);
            }
            else
                error = (Exception) Session["errorObject"];

            return View(error);
        }
    }
}