﻿using System;
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
        /// Предоставляет права для роли <see cref="roleName"/> для объекта <see cref="objectName"/>
        /// </summary>
        /// <param name="roleName">Имя роли</param>
        /// <param name="objectName">имя объекта</param>
        /// <param name="accessType">Тип доступа</param>
        /// <exception cref="ArgumentException">Возникает в случае отсутствия значений какого-либо из входных параметров в базе данных</exception>
        void Grant(string roleName, string objectName, SecurityAccessType accessType);

        void SetAccessTypes<T>();

        void SetAccessTypes<T1, T2>();

        void SetAccessTypes<T1, T2, T3>();

        void SetAccessTypes<T1, T2, T3, T4>();

        IPublicRole PublicRole { get; }

        void AddUser(string userName, string password, string email, string displayName, string sid);

        void AddGroup(string groupName, string description);

        void AddRole(string roleName, string description);

        void AddController(string path);

        void AddTable(string tableName);
        void SetRole(string roleName, string memberName);
        void SetGroup(string groupName, string login);
    }
}