using System;
using System.Security;

namespace SystemTools.Exceptions
{
    public class SecurityException2 : SecurityException
    {
        /// <summary>
        /// »нициализирует новый экземпл€р класса <see cref="T:System.Security.SecurityException"/> со свойствами по умолчанию.
        /// </summary>
        public SecurityException2()
            : base("ќбъект безопасности не инициализирован")
        {
        }

        /// <summary>
        /// »нициализирует новый экземпл€р класса <see cref="T:System.Security.SecurityException"/> с заданным сообщением об ошибке.
        /// </summary>
        /// <param name="message">—ообщение об ошибке с объ€снением причин исключени€.</param>
        public SecurityException2(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// »нициализирует новый экземпл€р класса <see cref="T:System.Security.SecurityException"/> с указанным сообщением об ошибке и ссылкой на внутреннее исключение, вызвавшее это исключение.
        /// </summary>
        /// <param name="message">—ообщение об ошибке с объ€снением причин исключени€.</param><param name="inner">»сключение, которое вызвало текущее исключение. ≈сли значение параметра <paramref name="inner"/> не равно null, текущее исключение создаетс€ в блоке catch, обрабатывающем внутреннее исключение.</param>
        public SecurityException2(Exception inner) 
            : base("ќшибка безопасности. —мотрите внутреннее сообщение.", inner)
        {
        }
    }
}