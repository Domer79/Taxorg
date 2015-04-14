using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using SystemTools.WebTools.Infrastructure;

namespace SystemTools.Interfaces
{
    public interface ISecurity
    {
        bool Sign(string login, string password);

        void CreateCookie(string login, bool isPersistent = false);

        IPrincipal GetWebPrinicipal();

        IPrincipal GetWindowsPrincipal(string name);

        /// <summary>
        /// Определяет есть ли права доступа у текущего участника к запрашиваемому объекту
        /// </summary>
        /// <param name="objectName">Объект, запрашивающий права доступа</param>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="accessType">Тип доступа</param>
        /// <returns></returns>
        bool IsAccess(string objectName, string userName, SecurityAccessType accessType);

        /// <summary>
        /// Предоставляет права роли <see cref="role"/> для запуска действия контроллера 
        /// </summary>
        /// <param name="controller">Имя контроллера</param>
        /// <param name="action">Имя дествия</param>
        /// <param name="role">Имя роли</param>
        /// <exception cref="ArgumentException">Возникает в случае отсутствия значений какого-либо из входных параметров в базе данных</exception>
        void GrantActionToRole(string controller, string action, string role);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="role"></param>
        /// <param name="accessType"></param>
        /// <exception cref="ArgumentException">Возникает в случае отсутствия значений какого-либо из входных параметров в базе данных</exception>
        void GrantEntityToRole(string entity, string role, SecurityAccessType accessType);

        void SetAccessTypes<T>();

        void SetAccessTypes<T1, T2>();

        void SetAccessTypes<T1, T2, T3>();

        void SetAccessTypes<T1, T2, T3, T4>();

        IPublicRole PublicRole { get; }
    }
}