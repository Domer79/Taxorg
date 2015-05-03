using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using SystemTools.Extensions;
using TaxorgRepository;

namespace TaxOrg.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class SysController : Controller
    {
        public ActionResult IsNotLoadSameTaxOnOff()
        {
            try
            {
                throw new Exception("Test exception");
                TaxorgTools.IsNotSameTaxLoad = !TaxorgTools.IsNotSameTaxLoad;
                return Json(TaxorgTools.IsNotSameTaxLoad);
//                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                e.SaveError();
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public ActionResult IsNotLoadSameTax()
        {
            try
            {
                return Json(TaxorgTools.IsNotSameTaxLoad);
            }
            catch (Exception e)
            {
                e.SaveError();
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public ActionResult SetTaxPrevPeriod(int taxPrevPeriod)
        {
            try
            {
                TaxorgTools.SetTaxPrevPeriodCount(taxPrevPeriod);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                e.SaveError();
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public ActionResult GetPrevPeriodName()
        {
            try
            {
                return Json(TaxorgTools.GetPrevPeriod().ToString());
            }
            catch (Exception e)
            {
                e.SaveError();
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}