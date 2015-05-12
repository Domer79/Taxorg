using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using SystemTools.Extensions;
using TaxorgRepository.Models;
using TaxorgRepository.Repositories;

namespace TaxOrg.Controllers
{
    public class KbkController : Controller
    {
        // GET: Kbk
        public async Task<ActionResult> Index()
        {
            var taxTypeRepository = new TaxTypeRepository();
            ViewBag.TaxTypeCollection = await GetTaxTypeCollectionTaskAsync(); 
            return View(taxTypeRepository);
        }

        private void SetTaxTypeCollection()
        {
            var taxTypeCollection =

            ViewBag.TaxTypeCollection = GetTaxTypeCollectionTaskAsync(); 
        }

        private async Task<IEnumerable<int>> GetTaxTypeCollectionTaskAsync()
        {
            var collection = await Task.Run(() => GetTaxTypeCollection());
            return collection;
        }

        private IEnumerable<int> GetTaxTypeCollection()
        {
            var repo = new Repository<SessionTaxType>();
            return repo.Where(stt => stt.SessionId == Session.SessionID).Select(stt => stt.IdTaxType);
        }

        /// <summary>
        /// Добавляет/удаляет idTaxType в SessionTaxType
        /// </summary>
        /// <param name="idTaxType"></param>
        /// <param name="added"></param>
        /// <returns></returns>
        public ActionResult SetStt(int idTaxType, bool added)
        {
            try
            {
                var sessionId = Session.SessionID;
                var repoStt = new Repository<SessionTaxType>();

                SessionTaxType sessionTaxType = null;
                //Если требуется добавить код КБК,
                if(added)
                    //а он уже есть для этой сессии, прерываем операцию и говорим, что операция прошла успешно.
                    if (repoStt.Any(stt => stt.SessionId == sessionId && stt.IdTaxType == idTaxType))
                        return new HttpStatusCodeResult(HttpStatusCode.OK);
                    //Иначе добавляем его в эту сессию
                    else
                    {
                        sessionTaxType = new SessionTaxType();
                        sessionTaxType.SessionId = sessionId;
                        sessionTaxType.IdTaxType = idTaxType;
                        repoStt.InsertOrUpdate(sessionTaxType);
                        repoStt.SaveChanges();
//                        if (Session["stt"] == null)
//                            Session.Add("stt", new List<int>{idTaxType});
//                        else
//                        {
//                            var list = (List<int>) Session["stt"];
//                            list.Add(idTaxType);
//                        }
                        return new HttpStatusCodeResult(HttpStatusCode.OK);
                    }

                //Иначе, 
                //удалем код КБК из этой сессии, если он есть
                sessionTaxType = repoStt.FirstOrDefault(stt => stt.SessionId == sessionId && stt.IdTaxType == idTaxType);
                if (sessionTaxType != null)
                {
                    repoStt.Delete(sessionTaxType);
                    repoStt.SaveChanges();
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                e.SaveError();
                var statusDescription = e.GetErrorMessage();
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, statusDescription.Substring(0, 399));
            }
        }
    }
}