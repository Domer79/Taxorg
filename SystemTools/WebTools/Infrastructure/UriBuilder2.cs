using System;

namespace SystemTools.WebTools.Infrastructure
{
    public class UriBuilder2
    {
        private UriBuilder _uriBuilder = new UriBuilder();
        private readonly PathCollection _paths = new PathCollection();
        private readonly QueryCollection _queries = new QueryCollection();

        #region CTOR

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.UriBuilder"/>.
        /// </summary>
        public UriBuilder2()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.UriBuilder"/>, используя указанный URI.
        /// </summary>
        /// <param name="uri">Строка URI.</param><exception cref="T:System.ArgumentNullException">Параметр <paramref name="uri"/> имеет значение null.</exception><exception cref="T:System.UriFormatException">В параметре <paramref name="uri"/> содержится строка нулевой длины или строка, состоящая только из пробелов. -или-  Подпрограмма синтаксического анализа обнаружила схему в недопустимой форме. -или-  Средство синтаксического анализа обнаружило более двух последовательно расположенных косых черт, для которых не используются схема File. -или-  Параметр <paramref name="uri"/> представляет недопустимый URI.</exception>
        public UriBuilder2(string uri)
        {
            UriBuilder = new UriBuilder(uri);
            Path = UriBuilder.Path;
            Query = UriBuilder.Query;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.UriBuilder"/> в соответствии с указанным экземпляром класса <see cref="T:System.Uri"/>.
        /// </summary>
        /// <param name="uri">Экземпляр класса <see cref="T:System.Uri"/>.</param><exception cref="T:System.ArgumentNullException">Параметр <paramref name="uri"/> имеет значение null.</exception>
        public UriBuilder2(Uri uri)
        {
            UriBuilder = new UriBuilder(uri);
            Path = UriBuilder.Path;
            Query = UriBuilder.Query;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.UriBuilder"/> в соответствии с заданной схемой и узлом.
        /// </summary>
        /// <param name="schemeName">Протокол доступа к Интернету.</param><param name="hostName">DNS-имя домена или IP-адрес.</param>
        public UriBuilder2(string schemeName, string hostName)
        {
            UriBuilder = new UriBuilder(schemeName, hostName);
            Path = UriBuilder.Path;
            Query = UriBuilder.Query;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.UriBuilder"/> в соответствии с заданной схемой, узлом и портом.
        /// </summary>
        /// <param name="scheme">Протокол доступа к Интернету.</param><param name="host">DNS-имя домена или IP-адрес.</param><param name="portNumber">Номер порта IP, используемый службой.</param><exception cref="T:System.ArgumentOutOfRangeException">Параметр <paramref name="portNumber"/> меньше -1 или больше 65535.</exception>
        public UriBuilder2(string scheme, string host, int portNumber)
        {
            UriBuilder = new UriBuilder(scheme, host, portNumber);
            Path = UriBuilder.Path;
            Query = UriBuilder.Query;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.UriBuilder"/> в соответствии с заданной схемой, узлом, номером порта и путем.
        /// </summary>
        /// <param name="scheme">Протокол доступа к Интернету.</param><param name="host">DNS-имя домена или IP-адрес.</param><param name="port">Номер порта IP, используемый службой.</param><param name="pathValue">Путь к Интернет- ресурсу.</param><exception cref="T:System.ArgumentOutOfRangeException">Параметр <paramref name="port"/> меньше -1 или больше 65535.</exception>
        public UriBuilder2(string scheme, string host, int port, string pathValue)
        {
            UriBuilder = new UriBuilder(scheme, host, port, pathValue);
            Path = UriBuilder.Path;
            Query = UriBuilder.Query;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.UriBuilder"/> в соответствии с заданной схемой, узлом, номером порта и строкой запроса или идентификатором фрагмента.
        /// </summary>
        /// <param name="scheme">Протокол доступа к Интернету.</param><param name="host">DNS-имя домена или IP-адрес.</param><param name="port">Номер порта IP, используемый службой.</param><param name="path">Путь к Интернет- ресурсу.</param><param name="extraValue">Строка запроса или идентификатор фрагмента.</param><exception cref="T:System.ArgumentException">Параметр <paramref name="extraValue"/> не принимает ни значение null, ни значение <see cref="F:System.String.Empty"/>, не является допустимым идентификатором фрагмента, начинающимся со знака решетки (#) и не является допустимой строкой запроса, начинающейся с вопросительного знака (?).</exception><exception cref="T:System.ArgumentOutOfRangeException">Параметр <paramref name="port"/> меньше -1 или больше 65535.</exception>
        public UriBuilder2(string scheme, string host, int port, string path, string extraValue)
        {
            UriBuilder = new UriBuilder(scheme, host, port, path, extraValue);
            Path = UriBuilder.Path;
            Query = UriBuilder.Query;
        }

        #endregion

        public void AddPath(string path)
        {
            _paths.Add(path);
        }

        public string Path
        {
            get { return _paths.Path; }
            set
            {
                _paths.Path = value;
                _uriBuilder.Path = _paths.Path;
            }
        }

        public string Query
        {
            get { return _queries.Query; }
            set
            {
                _queries.Query = value;
                _uriBuilder.Query = _queries.Query;
            }
        }

        public string Fragment
        {
            get { return _uriBuilder.Fragment; }
            set { _uriBuilder.Fragment = value; }
        }

        public string Host
        {
            get { return _uriBuilder.Host; }
            set { _uriBuilder.Host = value; }
        }

        public string Scheme
        {
            get { return _uriBuilder.Scheme; }
            set { _uriBuilder.Scheme = value; }
        }

        public int Port
        {
            get { return _uriBuilder.Port; }
            set { _uriBuilder.Port = value; }
        }

        public string UserName
        {
            get { return _uriBuilder.UserName; }
            set { _uriBuilder.UserName = value; }
        }

        public string Password
        {
            get { return _uriBuilder.Password; }
            set { _uriBuilder.Password = value; }
        }

        public Uri Uri
        {
            get { return UriBuilder.Uri; }
        }

        private string GenerateQuery()
        {
            return _queries.Query;
        }

        private string GeneratePath()
        {
            return _paths.Path;
        }

        private UriBuilder UriBuilder
        {
            get { return _uriBuilder; }
            set { _uriBuilder = value; }
        }
    }
}
