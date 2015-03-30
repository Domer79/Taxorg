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
        /// <summary>
        /// Извлекает экземпляр контроллера для заданного контекста запроса и типа контроллера.
        /// </summary>
        /// <returns>
        /// Экземпляр контроллера.
        /// </returns>
        /// <param name="requestContext">Контекст HTTP-запроса, включающий в себя контекст HTTP и данные маршрута.</param><param name="controllerType">Тип контроллера.</param><exception cref="T:System.Web.HttpException">Параметр <paramref name="controllerType"/> равен null.</exception><exception cref="T:System.ArgumentException">Невозможно присвоить тип <paramref name="controllerType"/>.</exception><exception cref="T:System.InvalidOperationException">Невозможно создать экземпляр <paramref name="controllerType"/>.</exception>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return base.GetControllerInstance(requestContext, controllerType);
        }
    }
}
