using System;
using System.Web;
using System.Web.Http;
using SystemTools;
using SystemTools.Exceptions;
using SystemTools.Extensions;
using SystemTools.WebTools.Infrastructure;
using TaxorgRepository;
using WebSecurity;

namespace TaxOrg.Controllers
{
    public class SecurityApiController : ApiController
    {
//        [System.Web.Http.Route("api/SecurityApi/GetLastError")]
        public IHttpActionResult PostLastError()
        {
            return Ok(TaxorgTools.GetLastError());
        }

        public IHttpActionResult GetUserList()
        {
            try
            {
                if (!ApplicationCustomizer.EnableSecurityAdminPanel && !Security.Instance.IsAccess("SecurityApiGetUserList", HttpContext.Current.User.Identity.Name, SecurityAccessType.Exec))
                    throw new ControllerActionAccessDeniedException("SecurityApi", "GetUserList");

                return Ok(ApplicationCustomizer.Security.GetUsers());
            }
            catch (Exception e)
            {
                e.SaveError();
                throw;
            }
        }
    }
}
