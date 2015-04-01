namespace SystemTools.EventArgs
{
    public class AuthenticatedEventArgs
    {
        private readonly string _login;
        private readonly string _password;

        public AuthenticatedEventArgs(string login, string password)
        {
            _login = login;
            _password = password;
        }

        public string Login
        {
            get { return _login; }
        }

        public string Password
        {
            get { return _password; }
        }
    }
}