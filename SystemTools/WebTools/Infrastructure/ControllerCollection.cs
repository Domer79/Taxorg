using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using SystemTools.Extensions;
using SystemTools.WebTools.Attributes;
using SystemTools.WebTools.Helpers;

namespace SystemTools.WebTools.Infrastructure
{
    public class ControllerCollection : IEnumerable<ControllerInfo>
    {
        private readonly Dictionary<MethodInfo, ControllerInfo> _controllerInfoList = new Dictionary<MethodInfo, ControllerInfo>();

        static ControllerCollection()
        {
            Assemblies = new List<Assembly>();
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.Object"/>.
        /// </summary>
        public ControllerCollection()
        {
            Init();
        }

        public static List<Assembly> Assemblies { get; private set; }

        private Dictionary<MethodInfo, ControllerInfo> ControllerInfoList
        {
            get { return _controllerInfoList; }
        }

        private void Init()
        {
            var types = Assemblies.SelectMany(a => a.GetTypes()).Where(t => t.GetInterface("IController") != null);

            foreach (var type in types)
            {
                Add(type);
            }
        }

        private static IEnumerable<MethodInfo> GetControllerMethods(Type type)
        {
            return type.GetMethods().Where(mi => mi.ReturnType.Is<ActionResult>() || IsTaskActionResult(mi.ReturnType));
        }

        private static bool IsTaskActionResult(Type returnType)
        {
            if (returnType.IsInterface)
                return false;
            check |= returnType.IsGenericTypeDefinition;
            if (check)
                check |= returnType.GetGenericTypeDefinition() != typeof (Task<>);

            check |= returnType.GetGenericArguments()[0].Is<ActionResult>();
//            if (returnType.IsGenericTypeDefinition && returnType.GetGenericTypeDefinition() != typeof (Task<>))
//                return false;

            return check;
        }

        public void Add<TController>() where TController : class, IController
        {
            Add(typeof(TController));
        }

        public void Add(Type controllerType)
        {
            foreach (var controllerMethod in GetControllerMethods(controllerType))
            {
                Add(controllerMethod);
            }
        }

        private void Add(MethodInfo methodInfo)
        {
            var controllerInfo = new ControllerInfo(methodInfo);

            if (ControllerInfoList.Values.Any(ci => string.Equals(ci.Alias, controllerInfo.Alias, StringComparison.InvariantCultureIgnoreCase)))
                return;

            ControllerInfoList.Add(methodInfo, controllerInfo);
        }

        public ControllerInfo this[MethodInfo methodInfo]
        {
            get { return ControllerInfoList[methodInfo]; }
        }

        public ControllerInfo GetControllerInfo(Type controllerType, MethodInfo methodInfo)
        {
            return GetControllerInfo(controllerType, methodInfo.Name);
        }

        public ControllerInfo GetControllerInfo(Type controllerType, string methodName)
        {
            var methodInfo =  ControllerInfoList.Keys.First(k => k.DeclaringType == controllerType && k.Name == methodName);
            return this[methodInfo];
        }

        /// <summary>
        /// Возвращает перечислитель, выполняющий итерацию в коллекции.
        /// </summary>
        /// <returns>
        /// Интерфейс <see cref="T:System.Collections.Generic.IEnumerator`1"/>, который может использоваться для перебора элементов коллекции.
        /// </returns>
        public IEnumerator<ControllerInfo> GetEnumerator()
        {
            return ControllerInfoList.Values.GetEnumerator();
        }

        /// <summary>
        /// Возвращает перечислитель, осуществляющий итерацию в коллекции.
        /// </summary>
        /// <returns>
        /// Объект <see cref="T:System.Collections.IEnumerator"/>, который может использоваться для перебора коллекции.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

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
