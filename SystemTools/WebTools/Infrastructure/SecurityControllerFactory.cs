using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
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
            var routeValueDictionary = requestContext.RouteData.Values;
            var controller = (string) routeValueDictionary["controller"];
            var action = (string) routeValueDictionary["action"];
            var controllerType = GetControllerType(requestContext, controllerName);
            var controllerInfo = ControllerHelper.ControllerCollection.GetControllerInfo(controllerType, action);

            if (string.Equals(requestContext.HttpContext.Request.HttpMethod, "POST",
                StringComparison.InvariantCultureIgnoreCase))
                return base.CreateController(requestContext, controllerName);

            //Если ошибка
            if (ApplicationCustomizer.IsError)
            {
                ApplicationCustomizer.IsError = false;
                return base.CreateController(requestContext, controllerName);
            }

            if (String.Equals(controllerName, ApplicationSettings.SecurityControllerName, StringComparison.CurrentCultureIgnoreCase) && ApplicationCustomizer.EnableSecurityAdminPanel)
            {
                return base.CreateController(requestContext, controllerName);
            }

            //Если безопасность отключена
            if (!ApplicationCustomizer.EnableSecurity)
                return base.CreateController(requestContext, controllerName);

            //Если пользователь не авторизован
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                //Если включена авторизация с помощью форм перенаправляем его на страницу авторизации
                if (FormsAuthentication.IsEnabled)
                {
                    controller = ApplicationSettings.SignPage.Controller;
                    action = ApplicationSettings.SignPage.Action;

                    routeValueDictionary.Clear();
                    routeValueDictionary.Add("controller", controller);
                    routeValueDictionary.Add("action", action);

                    return base.CreateController(requestContext, controller);
                }

                //Если запрещена идентификация анонимных пользователей, прекращаем работу
                if (!AnonymousIdentificationModule.Enabled)
                    throw new ControllerActionAccessDeniedException(controller, action);
            }

            #region Проверка прав пользователя

            var isAccess = ApplicationCustomizer.Security.IsAccess(controllerInfo.Alias,
                HttpContext.Current.User.Identity.Name, SecurityAccessType.Exec);

            if (!isAccess)
                throw new ControllerActionAccessDeniedException(controller, action);

            #endregion

            return base.CreateController(requestContext, controllerName);
        }

        #endregion
    }
}
