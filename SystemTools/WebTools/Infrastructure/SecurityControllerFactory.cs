using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using SystemTools.ConfigSections;
using SystemTools.Exceptions;
using SystemTools.Interfaces;
using SystemTools.WebTools.Helpers;

namespace SystemTools.WebTools.Infrastructure
{
    public class SecurityControllerFactory : DefaultControllerFactory
    {
        #region Overrides of DefaultControllerFactory

        /// <summary>
        /// Создает указанный контроллер, используя заданный контекст запроса.
        /// </summary>
        /// <returns>
        /// Контроллер.
        /// </returns>
        /// <param name="requestContext">Контекст HTTP-запроса, включающий в себя контекст HTTP и данные маршрута.</param><param name="controllerName">Имя контроллера.</param><exception cref="T:System.ArgumentNullException">Параметр <paramref name="requestContext"/> равен null.</exception><exception cref="T:System.ArgumentException">Параметр <paramref name="controllerName"/> имеет значение null или пуст.</exception>
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            RouteValueDictionary routeValueDictionary = requestContext.RouteData.Values;
            var controller = (string) routeValueDictionary["controller"];
            var action = (string) routeValueDictionary["action"];

            if (ApplicationCustomizer.IsError)
            {
                ApplicationCustomizer.IsError = false;
                return base.CreateController(requestContext, controllerName);
            }

            //Если пользователь не авторизован
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                //Если включена авторизация с помощью форм перенаправляем его на страницу авторизации
                if (FormsAuthentication.IsEnabled)
                {
                    controller = AdditionalConfiguration.Instance.SignPage.Controller;
                    action = AdditionalConfiguration.Instance.SignPage.Action;

                    routeValueDictionary.Clear();
                    routeValueDictionary.Add("controller", controller);
                    routeValueDictionary.Add("action", action);

                    return base.CreateController(requestContext, controller);
                }

                //Если запрещена идентификация анонимных пользователей, прекращаем работу
                if (!AnonymousIdentificationModule.Enabled)
                    throw new ControllerActionAccessDeniedException(controller, action);
            }

            var isAccess = ApplicationCustomizer.Security.IsAccess(ControllerHelper.GetActionPath(controller, action),
                HttpContext.Current.User.Identity.Name, SecurityAccessType.Exec);

            if (!isAccess)
                throw new ControllerActionAccessDeniedException(controller, action);

            return base.CreateController(requestContext, controllerName);
        }

        #endregion
    }
}
