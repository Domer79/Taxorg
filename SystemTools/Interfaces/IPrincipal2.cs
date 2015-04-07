using System;
using System.Security.Principal;

namespace SystemTools.Interfaces
{
    public interface IPrincipal2 : IPrincipal
    {
        /// <summary>
        /// ���������� ���� �� ����� ������� � �������� ��������� � �������������� �������
        /// </summary>
        /// <param name="objectName">������, ������������� ����� �������</param>
        /// <param name="accessType">��� �������</param>
        /// <returns></returns>
        bool IsAccess(string objectName, Enum accessType);
    }
}