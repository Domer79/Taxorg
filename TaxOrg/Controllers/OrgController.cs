﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using SystemTools.Extensions;
using SystemTools.WebTools.Attributes;
using SystemTools.WebTools.Helpers;
using SystemTools.WebTools.Infrastructure;
using TaxOrg.Tools;
using TaxorgRepository;
using TaxorgRepository.Models;
using TaxorgRepository.Repositories;

namespace TaxOrg.Controllers
{
    [SessionState(SessionStateBehavior.Required)]
    public class OrgController : Controller
    {
        private TaxSummaryRepository _repository;

        public OrgController()
        {
        }

        [ActionAlias("OrganizationTax", "Сгруппированные данные по налогам организации")]
        public ActionResult Index(GridSettings grid)
        {
            if (TaxorgTools.IsMaintenance)
                return RedirectToAction("Maintenance");

            _repository = new TaxSummaryRepository(Session.SessionID, grid);
            ViewBag.TotalTaxCount = _repository.Count;
            ViewBag.CurrentPeriod = TaxorgTools.GetCurrentPeriod().ToString();
            ViewBag.UserName = string.Format("{0}", HttpContext.User == null ? "анонимный" : HttpContext.User.Identity.Name);
            return View();
        }

        [ActionAlias("GetOrganizationTax", "Возвращает сгруппированные данные по налогам организации, по запросу Ajax")]
        public JsonResult GetData(GridSettings grid)
        {
            _repository = new TaxSummaryRepository(Session.SessionID, grid);
            var jsonData = Json(_repository.Data, JsonRequestBehavior.AllowGet);
            return jsonData;
        }

        [ActionAlias("OrganizationEdit", "Редактирование данных о организации")]
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

        [ActionAlias("OrganizationDelete", "Удаление организации")]
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
    }
}