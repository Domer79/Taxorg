using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using SystemTools.Extensions;

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

        private async void InitAsync()
        {
            var types = GetControllerTypes();

            await Task.Run(() =>
            {
                foreach (var type in types)
                {
                    Add(type);
                }
            });
        }

        private void Init()
        {
            var types = GetControllerTypes();

            foreach (var type in types)
            {
                Add(type);
            }
        }

        private static IEnumerable<Type> GetControllerTypes()
        {
            return Assemblies.SelectMany(a => a.GetTypes()).Where(t => t.GetInterface("IController") != null);
        }

        private static IEnumerable<MethodInfo> GetControllerMethods(Type type)
        {
            return type.GetMethods().Where(mi => mi.ReturnType.Is<ActionResult>() || IsTaskActionResult(mi.ReturnType));
        }

        private static bool IsTaskActionResult(Type returnType)
        {
            if (returnType.IsInterface)
                return false;

            if (!returnType.IsGenericType)
                return false;

            if (returnType.GetGenericTypeDefinition() != typeof (Task<>))
                return  false;

            if (!returnType.GetGenericArguments()[0].Is<ActionResult>())
                return false;

            return true;
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
}
