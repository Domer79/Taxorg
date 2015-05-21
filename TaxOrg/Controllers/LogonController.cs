using System.Web.Mvc;
using SystemTools;
using TaxOrg.Models;

namespace TaxOrg.Controllers
{
    public class LogonController : Controller
    {
        // GET: Logon
        public ActionResult Index()
        {
            return View(new LoginView());
        }

        [HttpPost]
        public ActionResult Index(LoginView loginView)
        {
            if (ModelState.IsValid)
            {
                if (!ApplicationCustomizer.Security.Sign(loginView.Login, loginView.Password))
                {
                    ModelState["Login"].Errors.Add("Пользователь не прошел проверку подлинности");
                    return View(loginView);
                }
            }

            ApplicationCustomizer.Security.CreateCookie(loginView.Login);
            return RedirectToAction("Index", "Org");
        }
    }
}