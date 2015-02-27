using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TaxOrg.Infrastructure;
using TaxOrg.Tools;
using TaxorgRepository.Models;
using TaxorgRepository.Repositories;

namespace TaxOrg.Controllers
{
    public class SliceController : Controller
    {
        // GET: Slice
        public ActionResult Index(int? idOrganization)
        {
            ViewBag.IdOrganization = idOrganization;

            if (idOrganization == null)
                return RedirectToAction("Index", "Org");

            return View();
        }

        public ActionResult GetData(int idOrganization, GridSettings grid)
        {
            var data = ControllerHelper.GetData(grid, new SliceRepository(idOrganization));
            return Json(data);
        }

        public ActionResult Edit(SliceTax sliceTax)
        {
            return new HttpStatusCodeResult(HttpStatusCode.HttpVersionNotSupported);
        }
    }
}