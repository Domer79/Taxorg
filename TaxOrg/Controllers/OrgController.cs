using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using SystemTools.Extensions;
using TaxOrg.Infrastructure;
using TaxOrg.Tools;
using TaxorgRepository;
using TaxorgRepository.Models;
using TaxorgRepository.Repositories;

namespace TaxOrg.Controllers
{
    [SessionState(SessionStateBehavior.Required)]
    public class OrgController : Controller
    {
        private readonly TaxSummaryRepository _repository;

        public OrgController()
        {
            _repository = new TaxSummaryRepository(System.Web.HttpContext.Current.Session.SessionID);
        }

//        [Authorize]
        public ActionResult Index()
        {
            if (TaxorgTools.IsMaintenance)
                return RedirectToAction("Maintenance");

            HttpContext.Session.Add("testObject", "Привет");
            
            

            ViewBag.TotalTaxCount = _repository.GetData().Count();
            ViewBag.CurrentPeriod = TaxorgTools.GetCurrentPeriod().ToString();
            ViewBag.PrevPeriod = TaxorgTools.GetPrevPeriod().ToString();
            ViewBag.UserName = string.Format("{0}", HttpContext.User == null ? "анонимный" : HttpContext.User.Identity.Name);
            return View();
        }

        public JsonResult GetData(GridSettings grid)
        {
            var jsonResult = ControllerHelper.GetData(grid, _repository.GetData(), "ShortName");
            var jsonData = Json(jsonResult);
            
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Response.Cache.SetNoServerCaching();
            HttpContext.Response.Cache.SetNoStore();
            HttpContext.Response.Cache.SetMaxAge(new TimeSpan(0, 0, 0, 5));
            HttpContext.Response.Cache.SetProxyMaxAge(new TimeSpan(0, 0, 0, 5));
            HttpContext.Response.Cache.SetOmitVaryStar(true);
            var now = DateTime.Now;
            HttpContext.Response.Cache.SetExpires(new DateTime(now.Ticks + 50000000));
            HttpContext.Response.Cache.SetNoTransforms();

            return jsonData;
        }

        public ActionResult Edit(TaxSummary editObject)
        {
            var orgRepository = new OrganizationRepository();
            var org = orgRepository.GetObjectByKey(editObject.IdOrganization);

            org.Inn = editObject.Inn;
            org.Name = editObject.Name;
            org.ShortName = editObject.ShortName;
            org.Address = editObject.Address;
//            orgRepository.InsertOrUpdate(org);

            try
            {
                orgRepository.SaveChanges();
            }
            catch (Exception e)
            {
                e.SaveError();
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var repo = new OrganizationRepository();
                repo.Delete(repo.GetObjectByKey(id));
                repo.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                e.SaveError();
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public ActionResult Maintenance()
        {
            if (!TaxorgTools.IsMaintenance)
                return RedirectToAction("Index");

            return View();
        }

        public ActionResult Error(string s)
        {
            return View(s);
        }
    }
}