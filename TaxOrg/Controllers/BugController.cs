using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using SystemTools.WebTools.Helpers;
using SystemTools.WebTools.Infrastructure;
using Microsoft.Ajax.Utilities;
using TaxOrg.Tools;
using TaxorgRepository;
using TaxorgRepository.Exceptions;
using TaxorgRepository.Models;
using TaxorgRepository.Repositories;
using SystemTools;
using Organization = TaxOrg.Tools.Organization;

namespace TaxOrg.Controllers
{
    public class BugController : Controller
    {
        private readonly BugRepository _repository = new BugRepository();
        // GET: Bug
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetData(GridSettings grid)
        {
            return Json(ControllerHelper.GetData(grid, _repository, _repository.GetKeyName()));
        }

        [HttpPost]
        public ActionResult Edit(Bug bug)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new HttpStatusCodeResult(HttpStatusCode.Conflict, ModelState.Values.Aggregate(
                            (state, current) =>
                            {
                                if (current.Errors.Count > 0)
                                {
                                    state.Errors.Add(current.Errors.Aggregate((error, eCurrent) => new ModelError(error.ErrorMessage + @". " + eCurrent.ErrorMessage)));
                                }
                                return state;
                            }).Errors.Aggregate((error, eCurrent) => new ModelError(error.ErrorMessage + @". " + eCurrent.ErrorMessage)).ErrorMessage);

                try
                {
                    //TODO Не забыть изменить на <<if (bug.Accept)>>
                    var csvRow = new CsvRow<Organization>(bug.ErrorData);
                    var org = csvRow.GetObject();
                    var taxRepository = new TaxRepository();
                    try
                    {
                        TaxorgTools.CheckSaveTaxAccess();
                        taxRepository.SaveTaxToDb(org.Inn, org.TaxCode, org.Date, org.Tax);
                        bug.Accept = true;
                    }
                    catch (Exception e)
                    {
                        throw new SaveTaxException(e.Message);
                    }

                    _repository.InsertOrUpdate(bug);
                    _repository.SaveChanges();
                }
                catch (Exception e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable, e.Message);
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable, e.Message); 
            }
        }

        public ActionResult IsLoadBug()
        {
            var notAccepted = !_repository.All(b => b.Accept);

            return Json(new { notAccepted = notAccepted });
        }

        public ActionResult Delete(int id)
        {
            _repository.Delete(_repository.GetObjectByKey(id));
            _repository.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}