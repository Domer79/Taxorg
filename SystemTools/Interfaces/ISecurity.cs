using System.Security.Principal;
using System.Web;

namespace SystemTools.Interfaces
{
    public interface ISecurity
    {
        bool Sign(string login, string password);

        void CreateCookie(string login, bool isPersistent = false);

        IPrincipal2 WebPrincipal { get; }
    }
}