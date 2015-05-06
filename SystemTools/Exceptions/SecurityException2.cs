using System;
using System.Security;

namespace SystemTools.Exceptions
{
    public class SecurityException2 : SecurityException
    {
        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="T:System.Security.SecurityException"/> �� ���������� �� ���������.
        /// </summary>
        public SecurityException2()
            : base("������ ������������ �� ���������������")
        {
        }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="T:System.Security.SecurityException"/> � �������� ���������� �� ������.
        /// </summary>
        /// <param name="message">��������� �� ������ � ����������� ������ ����������.</param>
        public SecurityException2(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="T:System.Security.SecurityException"/> � ��������� ���������� �� ������ � ������� �� ���������� ����������, ��������� ��� ����������.
        /// </summary>
        /// <param name="message">��������� �� ������ � ����������� ������ ����������.</param><param name="inner">����������, ������� ������� ������� ����������. ���� �������� ��������� <paramref name="inner"/> �� ����� null, ������� ���������� ��������� � ����� catch, �������������� ���������� ����������.</param>
        public SecurityException2(Exception inner) 
            : base("������ ������������. �������� ���������� ���������.", inner)
        {
        }
    }
}