using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using SystemTools;
using SystemTools.Exceptions;
using SystemTools.Extensions;
using SystemTools.Interfaces;
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

        public IHttpActionResult GetMemberList(string id)
        {
            try
            {
//                if (!ApplicationCustomizer.EnableSecurityAdminPanel && !Security.Instance.IsAccess("SecurityApiGetUserList", HttpContext.Current.User.Identity.Name, SecurityAccessType.Exec))
//                    throw new ControllerActionAccessDeniedException("SecurityApi", "GetMemberList");

                if (id.ToLower() == "users")
                    return Ok(ApplicationCustomizer.Security.GetUsers().Select(u => new {u.IdMember, u.Name}));
                if (id.ToLower() == "groups")
                    return Ok(ApplicationCustomizer.Security.GetGroups().Select(u => new { u.IdMember, u.Name }));

                throw new ArgumentException(id);
            }
            catch (Exception e)
            {
                e.SaveError();
                throw;
            }
        }
    }
}
