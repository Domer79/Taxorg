using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.SessionState;

namespace TaxOrg.Controllers
{
    public class ProtectManager : SessionIDManager
    {
        public override string CreateSessionID(HttpContext context)
        {
            string x = Guid.NewGuid().ToString().Replace("-", "").ToLower();
            string ip = context.Request.ServerVariables["REMOTE_ADDR"].Trim();
            string br = context.Request.ServerVariables["HTTP_USER_AGENT"].Trim();
            if ((ip != "") && (br != ""))
            {
                string id = crypt.getmd5(ip + br) + x;
                if (id.Length == 64) return id;
                else return x;
            }
            else return x;
        }

        public override bool Validate(string id)
        {
            return true;
        }
    }

    public class TaxorgSessionIdManager : ISessionIDManager
    {
        public const string HeaderName = "AspFilterSessionId";
        private SessionStateSection _pConfig = null;
        /// <summary>
        /// Выполняет инициализацию объекта <see cref="T:System.Web.SessionState.SessionIDManager"/> по запросу.
        /// </summary>
        /// <returns>
        /// Значение true, если нужно указать, что при инициализации было выполнено перенаправление, или значение false в противном случае.
        /// </returns>
        /// <param name="context">Объект <see cref="T:System.Web.HttpContext"/>, содержащий сведения о текущем запросе.</param><param name="suppressAutoDetectRedirect">Если диспетчер ИД сеансов должен выполнять перенаправления для определения поддержки файлов Cookie, значение true, если это не так, значение false, чтобы отключить автоматическое перенаправление для определения поддержки файлов Cookie.</param><param name="supportSessionIDReissue">При возврате этим методом содержит значение логического типа, позволяющее определить, поддерживает ли объект <see cref="T:System.Web.SessionState.ISessionIDManager"/> выпуск новых ИД сеанса при устаревании исходного ИД. Этот параметр передается неинициализированным. Повторное использование ИД сеанса приемлемо, если ИД состояния сеанса закодирован в URL-адресе и этот URL-адрес может использоваться для общего доступа или отправки по электронной почте. Если настраиваемая реализация состояния сеанса разделяет файлы Cookie по виртуальному пути, должно также поддерживаться состояние сеанса.</param>
        public bool InitializeRequest(HttpContext context, bool suppressAutoDetectRedirect, out bool supportSessionIDReissue)
        {
            if (_pConfig.Cookieless == HttpCookieMode.UseCookies)
            {
                supportSessionIDReissue = false;
                return false;
            }
            else
            {
                supportSessionIDReissue = true;
                return context.Response.IsRequestBeingRedirected;
            }

        }

        /// <summary>
        /// Возвращает идентификатор сеанса из контекста текущего HTTP-запроса.
        /// </summary>
        /// <returns>
        /// Текущий идентификатор сеанса, отправленный в HTTP-запросе.
        /// </returns>
        /// <param name="context">Текущий объект <see cref="T:System.Web.HttpContext"/>, содержащий ссылки на серверные объекты, используемые для обработки HTTP-запросов (например, свойства <see cref="P:System.Web.HttpContext.Request"/> и <see cref="P:System.Web.HttpContext.Response"/>).</param>
        public string GetSessionID(HttpContext context)
        {
            string id = null;

            if (_pConfig.Cookieless == HttpCookieMode.UseUri)
            {
                string tmp = context.Request.Headers[HeaderName];
                if (tmp != null)
                    id = Regex.Match(tmp, "[a-f|0-9]{8}-[a-f|0-9]{4}-[a-f|0-9]{4}-[a-f|0-9]{4}-[a-f|0-9]{12}", RegexOptions.IgnoreCase).Value;

                // Retrieve the SessionID from the URI.
            }
            else
            {
                if (context.Request.Cookies.Count > 0)
                {
                    var httpCookie = context.Request.Cookies[_pConfig.CookieName];
                    if (httpCookie != null)
                    {
                        id = httpCookie.Value;
                        id = HttpUtility.UrlDecode(id);
                    }
                }
            }

            // Verify that the retrieved SessionID is valid. If not, return null.

            if (!Validate(id))
                id = null;

            return id;

        }

        /// <summary>
        /// Создает уникальный идентификатор сеанса.
        /// </summary>
        /// <returns>
        /// Уникальный идентификатор сеанса.
        /// </returns>
        /// <param name="context">Текущий объект <see cref="T:System.Web.HttpContext"/>, содержащий ссылки на серверные объекты, используемые для обработки HTTP-запросов (например, свойства <see cref="P:System.Web.HttpContext.Request"/> и <see cref="P:System.Web.HttpContext.Response"/>).</param>
        public string CreateSessionID(HttpContext context)
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Сохраняет созданный новый идентификатор в HTTP-ответе.
        /// </summary>
        /// <param name="context">Текущий объект <see cref="T:System.Web.HttpContext"/>, содержащий ссылки на серверные объекты, используемые для обработки HTTP-запросов (например, свойства <see cref="P:System.Web.HttpContext.Request"/> и <see cref="P:System.Web.HttpContext.Response"/>).</param><param name="id">Идентификатор сеанса.</param><param name="redirected">При возврате этим методом содержит логическое значение, равное true, если ответ перенаправляется по текущему URL-адресу с добавленным в него идентификатором сеанса, или значение false в противном случае.</param><param name="cookieAdded">При возврате этим методом содержит логическое значение, равное true, если в HTTP-ответ добавлен файл Cookie, или значение false в противном случае.</param>
        public void SaveSessionID(HttpContext context, string id, out bool redirected, out bool cookieAdded)
        {
            if (!Validate(id))
                throw new HttpException("Invalid session ID");

            redirected = false;
            cookieAdded = false;

            if (_pConfig.Cookieless == HttpCookieMode.UseUri)
            {
                // Add the SessionID to the URI. Set the redirected variable as appropriate.

                //context.Request.Headers.Add(HeaderName, id);
                //context.Request.Headers.Set(HeaderName, id);
//                SetHeader(HeaderName, id);

                redirected = true;
                UriBuilder newUri = new UriBuilder(context.Request.Url);
//                newUri.Path = InsertSessionId(id, context.Request.FilePath);
                newUri.Path = string.Format("/(S({0})){1}", id, context.Request.FilePath);

                //http://localhost:52897/(S(sq2abm453wnasg45pvboee45))/DisplaySessionValues.aspx
                context.Response.Redirect(newUri.Uri.PathAndQuery, false);
                context.ApplicationInstance.CompleteRequest(); // Important !
            }
            else
            {
                var httpCookie = new HttpCookie(_pConfig.CookieName, id);
//                httpCookie.HttpOnly = true;
//                httpCookie.Secure = true;
                context.Response.Cookies.Add(httpCookie);
                cookieAdded = true;
            }

        }

        /// <summary>
        /// Удаляет идентификатор сеанса из файла Cookie или URL-адреса.
        /// </summary>
        /// <param name="context">Текущий объект <see cref="T:System.Web.HttpContext"/>, содержащий ссылки на серверные объекты, используемые для обработки HTTP-запросов (например, свойства <see cref="P:System.Web.HttpContext.Request"/> и <see cref="P:System.Web.HttpContext.Response"/>).</param>
        public void RemoveSessionID(HttpContext context)
        {
            //context.Response.Cookies.Remove(_pConfig.CookieName);
        }

        /// <summary>
        /// Подтверждает допустимость предоставленного идентификатора сеанса.
        /// </summary>
        /// <returns>
        /// Если идентификатор сеанса допустим, значение true, если нет, значение false.
        /// </returns>
        /// <param name="id">Идентификатор сеанса для проверки.</param>
        public bool Validate(string id)
        {
            try
            {
                Guid testGuid = new Guid(id);

                if (id == testGuid.ToString())
                    return true;
            }
            catch
            {
            }

            return false;

        }

        /// <summary>
        /// Инициализирует объект <see cref="T:System.Web.SessionState.SessionIDManager"/>.
        /// </summary>
        public void Initialize()
        {
            if (_pConfig == null)
            {
                var cfg = WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
                _pConfig = (SessionStateSection)cfg.GetSection("system.web/sessionState");
            }
        }
    }

    public class crypt
    {
        public static string getmd5(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }



}
