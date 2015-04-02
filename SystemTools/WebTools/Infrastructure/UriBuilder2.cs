using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace SystemTools.WebTools.Infrastructure
{
    public class UriBuilder2
    {
        private string _uriPath;
        private StringBuilder _uriStringBuilder = new StringBuilder();
        private UriBuilder _uriBuilder = new UriBuilder();
        private readonly PathCollection _paths = new PathCollection();
        private readonly List<string> _queries = new List<string>();

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
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.UriBuilder"/> в соответствии с указанным экземпляром класса <see cref="T:System.Uri"/>.
        /// </summary>
        /// <param name="uri">Экземпляр класса <see cref="T:System.Uri"/>.</param><exception cref="T:System.ArgumentNullException">Параметр <paramref name="uri"/> имеет значение null.</exception>
        public UriBuilder2(Uri uri)
        {
            UriBuilder = new UriBuilder(uri);
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.UriBuilder"/> в соответствии с заданной схемой и узлом.
        /// </summary>
        /// <param name="schemeName">Протокол доступа к Интернету.</param><param name="hostName">DNS-имя домена или IP-адрес.</param>
        public UriBuilder2(string schemeName, string hostName)
        {
            UriBuilder = new UriBuilder(schemeName, hostName);
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.UriBuilder"/> в соответствии с заданной схемой, узлом и портом.
        /// </summary>
        /// <param name="scheme">Протокол доступа к Интернету.</param><param name="host">DNS-имя домена или IP-адрес.</param><param name="portNumber">Номер порта IP, используемый службой.</param><exception cref="T:System.ArgumentOutOfRangeException">Параметр <paramref name="portNumber"/> меньше -1 или больше 65535.</exception>
        public UriBuilder2(string scheme, string host, int portNumber)
        {
            UriBuilder = new UriBuilder(scheme, host, portNumber);
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.UriBuilder"/> в соответствии с заданной схемой, узлом, номером порта и путем.
        /// </summary>
        /// <param name="scheme">Протокол доступа к Интернету.</param><param name="host">DNS-имя домена или IP-адрес.</param><param name="port">Номер порта IP, используемый службой.</param><param name="pathValue">Путь к Интернет- ресурсу.</param><exception cref="T:System.ArgumentOutOfRangeException">Параметр <paramref name="port"/> меньше -1 или больше 65535.</exception>
        public UriBuilder2(string scheme, string host, int port, string pathValue)
        {
            UriBuilder = new UriBuilder(scheme, host, port, pathValue);
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.UriBuilder"/> в соответствии с заданной схемой, узлом, номером порта и строкой запроса или идентификатором фрагмента.
        /// </summary>
        /// <param name="scheme">Протокол доступа к Интернету.</param><param name="host">DNS-имя домена или IP-адрес.</param><param name="port">Номер порта IP, используемый службой.</param><param name="path">Путь к Интернет- ресурсу.</param><param name="extraValue">Строка запроса или идентификатор фрагмента.</param><exception cref="T:System.ArgumentException">Параметр <paramref name="extraValue"/> не принимает ни значение null, ни значение <see cref="F:System.String.Empty"/>, не является допустимым идентификатором фрагмента, начинающимся со знака решетки (#) и не является допустимой строкой запроса, начинающейся с вопросительного знака (?).</exception><exception cref="T:System.ArgumentOutOfRangeException">Параметр <paramref name="port"/> меньше -1 или больше 65535.</exception>
        public UriBuilder2(string scheme, string host, int port, string path, string extraValue)
        {
            UriBuilder = new UriBuilder(scheme, host, port, path, extraValue);
        }

        #endregion

        public void AddPath(string path)
        {
            _paths.Add(path);
        }

        public Uri Uri
        {
            get
            {
                UriBuilder.Path = GeneratePath();
                UriBuilder.Query = GenerateQuery();
                return UriBuilder.Uri;
            }
        }

        private string GenerateQuery()
        {
            throw new NotImplementedException();
        }

        private string GeneratePath()
        {
            throw new NotImplementedException();
        }

        private UriBuilder UriBuilder
        {
            get { return _uriBuilder; }
            set
            {
                _paths.Add(value.Path);
                _queries.AddRange(GetQueries(value));
                _uriBuilder = value;
            }
        }

        private static IEnumerable<string> GetPaths(UriBuilder uriBuilder)
        {
            return uriBuilder.Path.Split('/');
        }

        private static IEnumerable<string> GetQueries(UriBuilder uriBuilder)
        {
            if (string.IsNullOrEmpty(uriBuilder.Query))
                return new string[] {};

            var query = uriBuilder.Query.Substring(1);

            return query.Split('&');
        }
    }

    public class PathCollection : ICollection<string>
    {
        const string Pattern = "[^/].+[^/]";
        private readonly List<string> _paths = new List<string>();

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.Object"/>.
        /// </summary>
        public PathCollection()
        {
        }

        public PathCollection(string paths)
        {
            Add(paths);
        }

        /// <summary>
        /// Возвращает перечислитель, выполняющий итерацию в коллекции.
        /// </summary>
        /// <returns>
        /// Интерфейс <see cref="T:System.Collections.Generic.IEnumerator`1"/>, который может использоваться для перебора элементов коллекции.
        /// </returns>
        public IEnumerator<string> GetEnumerator()
        {
            return Paths.GetEnumerator();
        }

        /// <summary>
        /// Возвращает перечислитель, осуществляющий итерацию в коллекции.
        /// </summary>
        /// <returns>
        /// Объект <see cref="T:System.Collections.IEnumerator"/>, который может использоваться для перебора коллекции.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Добавляет элемент в коллекцию <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">Объект, добавляемый в коллекцию <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">Объект <see cref="T:System.Collections.Generic.ICollection`1"/> доступен только для чтения.</exception>
        public void Add(string item)
        {
            var path = SplitPath(item);

            if (path.Length == 0)
                return;

            if (path.Length > 1)
                AddRange(path);
            else
                Paths.Add(path[0]);
        }

        private static string[] SplitPath(string item)
        {
            var rx = new Regex(Pattern, RegexOptions.IgnoreCase);
            var path = rx.Match(item).Value.Split('/');
            return path;
        }

        private void AddRange(IEnumerable<string> items)
        {
            if (items == null) 
                throw new ArgumentNullException("items");

            foreach (var item in items)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Удаляет все элементы из коллекции <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">Объект <see cref="T:System.Collections.Generic.ICollection`1"/> доступен только для чтения.</exception>
        public void Clear()
        {
            Paths.Clear();
        }

        /// <summary>
        /// Определяет, содержит ли коллекция <see cref="T:System.Collections.Generic.ICollection`1"/> указанное значение.
        /// </summary>
        /// <returns>
        /// Значение true, если параметр <paramref name="item"/> найден в коллекции <see cref="T:System.Collections.Generic.ICollection`1"/>; в противном случае — значение false.
        /// </returns>
        /// <param name="item">Объект, который требуется найти в <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(string item)
        {
            var paths = SplitPath(item);
            if (paths.Length == 0)
                return false;

            var index = Paths.IndexOf(paths[0]);
            if (index == -1)
                return false;

            foreach (var path in paths.Except(new[] {paths[0]}))
            {
                if (Paths.IndexOf(path) != -1)
                    if (Math.Abs(Paths.IndexOf(path) - index) != 1)
                        return false;
                    else
                    {
                        index = Paths.IndexOf(path);
                    }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Копирует элементы <see cref="T:System.Collections.Generic.ICollection`1"/> в массив <see cref="T:System.Array"/>, начиная с указанного индекса <see cref="T:System.Array"/>.
        /// </summary>
        /// <param name="array">Одномерный массив <see cref="T:System.Array"/>, в который копируются элементы из интерфейса <see cref="T:System.Collections.Generic.ICollection`1"/>. Массив <see cref="T:System.Array"/> должен иметь индексацию, начинающуюся с нуля.</param><param name="arrayIndex">Отсчитываемый от нуля индекс в массиве <paramref name="array"/>, указывающий начало копирования.</param><exception cref="T:System.ArgumentNullException">Параметр <paramref name="array"/> имеет значение null.</exception><exception cref="T:System.ArgumentOutOfRangeException">Значение параметра <paramref name="arrayIndex"/> меньше 0.</exception><exception cref="T:System.ArgumentException">Количество элементов в исходной коллекции <see cref="T:System.Collections.Generic.ICollection`1"/> превышает доступное место, начиная с индекса <paramref name="arrayIndex"/> до конца массива назначения <paramref name="array"/>.</exception>
        public void CopyTo(string[] array, int arrayIndex)
        {
            Paths.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Удаляет первый экземпляр указанного объекта из коллекции <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// Значение true, если элемент <paramref name="item"/> успешно удален из <see cref="T:System.Collections.Generic.ICollection`1"/>, в противном случае — значение false. Этот метод также возвращает значение false, если параметр <paramref name="item"/> не найден в исходном интерфейсе <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">Объект, который необходимо удалить из коллекции <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">Объект <see cref="T:System.Collections.Generic.ICollection`1"/> доступен только для чтения.</exception>
        public bool Remove(string item)
        {
            var paths = SplitPath(item);
            return paths.All(path => Paths.Remove(path));
        }

        /// <summary>
        /// Получает число элементов, содержащихся в интерфейсе <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// Число элементов, содержащихся в интерфейсе <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count
        {
            get { return Paths.Count; }
        }

        /// <summary>
        /// Получает значение, указывающее, доступна ли <see cref="T:System.Collections.Generic.ICollection`1"/> только для чтения.
        /// </summary>
        /// <returns>
        /// Значение true, если <see cref="T:System.Collections.Generic.ICollection`1"/> доступна только для чтения; в противном случае — значение false.
        /// </returns>
        public bool IsReadOnly
        {
            get { return false; }
        }

        private List<string> Paths
        {
            get { return _paths; }
        }
    }
}
