using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

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

            ApplicationCustomizer.Security.WebPrincipal.IsAccess(routeValueDictionary[])

            return base.CreateController(requestContext, controllerName);
        }

        #endregion
    }
}
