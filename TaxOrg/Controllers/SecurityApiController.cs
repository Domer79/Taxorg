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
                switch (id.ToLower())
                {
                    case "users":
                        return Ok(ApplicationCustomizer.Security.GetUsers().Select(u => new {Id = u.IdMember, u.Name, u.DisplayName, u.Email, u.Usersid}));
                    case "groups":
                        return Ok(ApplicationCustomizer.Security.GetGroups().Select(g => new { Id = g.IdMember, g.Name, g.Description }));
                    case "roles":
                        return Ok(ApplicationCustomizer.Security.GetRoles().Select(r => new { Id = r.IdRole, Name = r.RoleName, r.Description }));
                }

                throw new ArgumentException(id);
            }
            catch (Exception e)
            {
                e.SaveError();
                throw;
            }
        }

        public IHttpActionResult GetUserGroups(int id)
        {
            return Ok(ApplicationCustomizer.Security.GetUserGroups().Where(ug => ug.IdUser == id).Select(g => new { g.IdGroup, g.GroupName, g.Description }));
        }

        public IHttpActionResult GetMemberRoles(int id)
        {
            return Ok(ApplicationCustomizer.Security.GetUserRoles(id).Select(r => new { r.IdRole, r.RoleName, r.Description }));
        }

        public IHttpActionResult GetGroupUsers(int id)
        {
            return Ok(ApplicationCustomizer.Security.GetUserGroups()
                        .Where(ug => ug.IdGroup == id)
                        .Select(u => new {u.Login, u.DisplayName}));
        }

        public IHttpActionResult GetMembers(int id)
        {
            return Ok(ApplicationCustomizer.Security.GetMembersByRole(id).Select(rm => new { rm.IdMember, rm.Name, rm.IsUser }));
        }
    }
}
