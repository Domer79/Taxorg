using System;
using System.Net;
using System.Web.Mvc;
using SystemTools.Extensions;
using TaxOrg.Infrastructure;
using TaxOrg.Tools;
using TaxorgRepository.Models;
using TaxorgRepository.Repositories;

namespace TaxOrg.Controllers
{
    public class SettingsController : Controller
    {
        private readonly RepositoryBase<Settings> _repo = new Repository<Settings>();
        // GET: Settings
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData(GridSettings grid)
        {
            return Json(ControllerHelper.GetData(grid, _repo));
        }

        public ActionResult Edit(Settings settings)
        {
            try
            {
                _repo.InsertOrUpdate(settings);
                _repo.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                e.SaveError();
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
    }
}