using System;

namespace SystemTools.Exceptions
{
    public class LoginEmailOrPasswordInvalidException : Exception
    {
        public LoginEmailOrPasswordInvalidException() 
            : base("Логин, email или пароль неверны")
        {
        }
    }
}