using System.Web.Http;
using TaxorgRepository;

namespace TaxOrg.Controllers
{
    public class SecurityApiController : ApiController
    {
//        [System.Web.Http.Route("api/SecurityApi/GetLastError")]
        public IHttpActionResult PostLastError()
        {
            return Ok(TaxorgTools.GetLastError());
        }
    }
}
