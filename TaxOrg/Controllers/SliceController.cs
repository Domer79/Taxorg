using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SystemTools.WebTools.Helpers;
using SystemTools.WebTools.Infrastructure;
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
            if (idOrganization == null)
                return RedirectToAction("Index", "Org");

            var repo = new OrganizationRepository();
            var organization = repo[idOrganization.Value];

            return View(organization);
        }

        public ActionResult GetData(int idOrganization, GridSettings grid)
        {
            var sliceRepository = new SliceRepository(idOrganization);
            var data = ControllerHelper.GetData(grid, sliceRepository, sliceRepository.GetKeyName());
            return Json(data);
        }

        public ActionResult Edit(SliceTax sliceTax)
        {
            return new HttpStatusCodeResult(HttpStatusCode.HttpVersionNotSupported);
        }
    }
}