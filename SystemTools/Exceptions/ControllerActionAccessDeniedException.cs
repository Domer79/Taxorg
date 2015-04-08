using System;

namespace SystemTools.Exceptions
{
    public class ControllerActionAccessDeniedException : UnauthorizedAccessException
    {
        /// <summary>
        /// ��������� ������������� ������ ���������� ������ <see cref="T:System.Exception"/>, ��������� ��������� ��������� �� ������.
        /// </summary>
        /// <param name="controller">��� �����������</param>
        /// <param name="action">��� ��������</param>
        public ControllerActionAccessDeniedException(string controller, string action) 
            : base(string.Format("������ �� ��������. Controller: {0}, action: {1}", controller, action))
        {
        }
    }
}