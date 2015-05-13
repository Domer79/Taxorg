using System;
using System.Reflection;
using System.Text.RegularExpressions;
using SystemTools.Extensions;
using SystemTools.WebTools.Attributes;
using SystemTools.WebTools.Helpers;

namespace SystemTools.WebTools.Infrastructure
{
    public class ControllerInfo
    {
        private readonly MethodInfo _methodInfo;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.Object"/>.
        /// </summary>
        public ControllerInfo()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.Object"/>.
        /// </summary>
        public ControllerInfo(MethodInfo methodInfo)
        {
            if (methodInfo == null) 
                throw new ArgumentNullException("methodInfo");

            if (methodInfo.DeclaringType == null)
                throw new InvalidOperationException("У метода отсутствует объявляющий его тип");

            _methodInfo = methodInfo;
        }

        public string Controller
        {
            get
            {
                const string pattern = @"(?i)(?<name>\w+)(controller)";

                if (!ControllerFullName.RxIsMatch(pattern))
                    throw new InvalidOperationException("Имя контроллера не соответствует соглашению");

                var rx = new Regex(pattern);
                return rx.Match(ControllerFullName).Groups["name"].Value;

            }
        }

        internal string ControllerFullName
        {
// ReSharper disable once PossibleNullReferenceException
            get { return MethodInfo.DeclaringType.Name; }
        }

        public string Action
        {
            get { return MethodInfo.Name; }
        }

        public string Alias
        {
            get
            {
                return ActionAliasAttribute != null ? ActionAliasAttribute.Alias : ControllerActionName;
            }
        }

        private string ControllerActionName
        {
            get { return string.Format("{0}{1}", Controller, Action); }
        }

        public AliasAttributeBase ActionAliasAttribute
        {
            get
            {
                var aliasAttribute = ((ActionAliasAttribute) Attribute.GetCustomAttribute(MethodInfo, typeof (ActionAliasAttribute)));
                return aliasAttribute ?? new ActionAliasAttribute(ControllerActionName);
            }
        }

        public string Description
        {
            get { return ActionAliasAttribute != null ? ActionAliasAttribute.Description : null; }
        }

        public MethodInfo MethodInfo
        {
            get { return _methodInfo; }
        }

        /// <summary>
        /// Возвращает строку, которая представляет текущий объект.
        /// </summary>
        /// <returns>
        /// Строка, представляющая текущий объект.
        /// </returns>
        public override string ToString()
        {
            return ControllerHelper.GetActionPath(Controller, Action);
        }
    }
}