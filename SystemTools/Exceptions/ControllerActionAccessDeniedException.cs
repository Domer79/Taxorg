using System;
using System.Net;
using System.Web;

namespace SystemTools.Exceptions
{
    public class ControllerActionAccessDeniedException : HttpException
    {

        /// <summary>
        /// ��������� ������������� ������ ���������� ������ <see cref="T:System.Exception"/>, ��������� ��������� ��������� �� ������.
        /// </summary>
        /// <param name="controller">��� �����������</param>
        /// <param name="action">��� ��������</param>
        public ControllerActionAccessDeniedException(string controller, string action) 
            : base((int)HttpStatusCode.Unauthorized, string.Format("������ �� ��������. Controller: {0}, action: {1}", controller, action))
        {
        }


    }
}