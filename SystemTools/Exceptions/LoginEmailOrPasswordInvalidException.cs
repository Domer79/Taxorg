using System;

namespace SystemTools.Exceptions
{
    public class LoginEmailOrPasswordInvalidException : Exception
    {
        public LoginEmailOrPasswordInvalidException() 
            : base("�����, email ��� ������ �������")
        {
        }
    }
}