using System;

namespace SystemTools.Exceptions
{
    public class ControllerActionAccessDeniedException : UnauthorizedAccessException
    {
        /// <summary>
        /// Выполняет инициализацию нового экземпляра класса <see cref="T:System.Exception"/>, используя указанное сообщение об ошибке.
        /// </summary>
        /// <param name="controller">Имя контроллера</param>
        /// <param name="action">Имя действия</param>
        public ControllerActionAccessDeniedException(string controller, string action) 
            : base(string.Format("Доступ не разрешен. Controller: {0}, action: {1}", controller, action))
        {
        }
    }
}