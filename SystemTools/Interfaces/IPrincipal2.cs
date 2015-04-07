using System;
using System.Security.Principal;

namespace SystemTools.Interfaces
{
    public interface IPrincipal2 : IPrincipal
    {
        /// <summary>
        /// Определяет есть ли права доступа у текущего участника к запрашиваемому объекту
        /// </summary>
        /// <param name="objectName">Объект, запрашивающий права доступа</param>
        /// <param name="accessType">Тип доступа</param>
        /// <returns></returns>
        bool IsAccess(string objectName, Enum accessType);
    }
}