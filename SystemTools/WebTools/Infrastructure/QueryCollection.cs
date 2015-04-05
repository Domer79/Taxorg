using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SystemTools.WebTools.Infrastructure
{
    internal class QueryCollection : IDictionary<string, string>
    {
        private const string Pattern = @"^?(?<name>[\w]+)=(?<value>[\w]*)";
        private readonly Dictionary<string, string> _queries = new Dictionary<string, string>();

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.Object"/>.
        /// </summary>
        public QueryCollection()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.Object"/>.
        /// </summary>
        public QueryCollection(string query)
        {
            Add(query);
        }

        /// <summary>
        /// Возвращает перечислитель, выполняющий итерацию в коллекции.
        /// </summary>
        /// <returns>
        /// Интерфейс <see cref="T:System.Collections.Generic.IEnumerator`1"/>, который может использоваться для перебора элементов коллекции.
        /// </returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _queries.GetEnumerator();
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

        public void Add(string query)
        {
            if (query == null) 
                throw new ArgumentNullException("query");

            var matchCollection = Regex.Matches(query, Pattern);

            foreach (var match in matchCollection.OfType<Match>())
            {
                _queries.Add(match.Groups["name"].Value, match.Groups["value"].Value);
            }
        }

        public string Query
        {
            get
            {
                var s = string.Empty;
                return _queries.Aggregate(s, (s1, keyValue) => string.Format("{0}&{1}={2}", s1, keyValue.Key, keyValue.Value), s1 => s1.Length > 0 ? s1.Substring(1) : s1);
            }
            set
            {
                _queries.Clear();
                Add(value);
            }
        }

        /// <summary>
        /// Добавляет элемент в коллекцию <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">Объект, добавляемый в коллекцию <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">Объект <see cref="T:System.Collections.Generic.ICollection`1"/> доступен только для чтения.</exception>
        void ICollection<KeyValuePair<string, string>>.Add(KeyValuePair<string, string> item)
        {
            _queries.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Удаляет все элементы из коллекции <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">Объект <see cref="T:System.Collections.Generic.ICollection`1"/> доступен только для чтения.</exception>
        public void Clear()
        {
            _queries.Clear();
        }

        /// <summary>
        /// Определяет, содержит ли коллекция <see cref="T:System.Collections.Generic.ICollection`1"/> указанное значение.
        /// </summary>
        /// <returns>
        /// Значение true, если параметр <paramref name="item"/> найден в коллекции <see cref="T:System.Collections.Generic.ICollection`1"/>; в противном случае — значение false.
        /// </returns>
        /// <param name="item">Объект, который требуется найти в <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(KeyValuePair<string, string> item)
        {
            return _queries.ContainsKey(item.Key);
        }

        /// <summary>
        /// Копирует элементы <see cref="T:System.Collections.Generic.ICollection`1"/> в массив <see cref="T:System.Array"/>, начиная с указанного индекса <see cref="T:System.Array"/>.
        /// </summary>
        /// <param name="array">Одномерный массив <see cref="T:System.Array"/>, в который копируются элементы из интерфейса <see cref="T:System.Collections.Generic.ICollection`1"/>. Массив <see cref="T:System.Array"/> должен иметь индексацию, начинающуюся с нуля.</param><param name="arrayIndex">Отсчитываемый от нуля индекс в массиве <paramref name="array"/>, указывающий начало копирования.</param><exception cref="T:System.ArgumentNullException">Параметр <paramref name="array"/> имеет значение null.</exception><exception cref="T:System.ArgumentOutOfRangeException">Значение параметра <paramref name="arrayIndex"/> меньше 0.</exception><exception cref="T:System.ArgumentException">Количество элементов в исходной коллекции <see cref="T:System.Collections.Generic.ICollection`1"/> превышает доступное место, начиная с индекса <paramref name="arrayIndex"/> до конца массива назначения <paramref name="array"/>.</exception>
        void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            if (array == null) 
                throw new ArgumentNullException("array");
            Array.Copy(_queries.ToArray(), arrayIndex, array, arrayIndex, array.Length - arrayIndex);
        }

        /// <summary>
        /// Удаляет первый экземпляр указанного объекта из коллекции <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// Значение true, если элемент <paramref name="item"/> успешно удален из <see cref="T:System.Collections.Generic.ICollection`1"/>, в противном случае — значение false. Этот метод также возвращает значение false, если параметр <paramref name="item"/> не найден в исходном интерфейсе <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">Объект, который необходимо удалить из коллекции <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">Объект <see cref="T:System.Collections.Generic.ICollection`1"/> доступен только для чтения.</exception>
        public bool Remove(KeyValuePair<string, string> item)
        {
            return _queries.Remove(item.Key);
        }

        /// <summary>
        /// Получает число элементов, содержащихся в интерфейсе <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// Число элементов, содержащихся в интерфейсе <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count
        {
            get { return _queries.Count; }
        }

        /// <summary>
        /// Получает значение, указывающее, доступна ли <see cref="T:System.Collections.Generic.ICollection`1"/> только для чтения.
        /// </summary>
        /// <returns>
        /// Значение true, если <see cref="T:System.Collections.Generic.ICollection`1"/> доступна только для чтения; в противном случае — значение false.
        /// </returns>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// Определяет, содержится ли элемент с указанным ключом в <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// Значение true, если в <see cref="T:System.Collections.Generic.IDictionary`2"/> содержится элемент с данным ключом; в противном случае — значение false.
        /// </returns>
        /// <param name="key">Ключ, который требуется найти в <see cref="T:System.Collections.Generic.IDictionary`2"/>.</param><exception cref="T:System.ArgumentNullException">Параметр <paramref name="key"/> имеет значение null.</exception>
        public bool ContainsKey(string key)
        {
            return _queries.ContainsKey(key);
        }

        /// <summary>
        /// Добавляет элемент с указанными ключом и значением в <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <param name="key">Объект, используемый в качестве ключа добавляемого элемента.</param><param name="value">Объект, используемый в качестве значения добавляемого элемента.</param><exception cref="T:System.ArgumentNullException">Параметр <paramref name="key"/> имеет значение null.</exception><exception cref="T:System.ArgumentException">Элемент с таким ключом уже существует в <see cref="T:System.Collections.Generic.IDictionary`2"/>.</exception><exception cref="T:System.NotSupportedException">Объект <see cref="T:System.Collections.Generic.IDictionary`2"/> доступен только для чтения.</exception>
        public void Add(string key, string value)
        {
            _queries.Add(key, value);
        }

        /// <summary>
        /// Удаляет элемент с указанным ключом из <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// Значение true, если элемент успешно удален, в противном случае — значение false. Этот метод возвращает также false, если <paramref name="key"/> не был найден в исходном <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        /// <param name="key">Ключ удаляемого элемента.</param><exception cref="T:System.ArgumentNullException">Параметр <paramref name="key"/> имеет значение null.</exception><exception cref="T:System.NotSupportedException">Объект <see cref="T:System.Collections.Generic.IDictionary`2"/> доступен только для чтения.</exception>
        public bool Remove(string key)
        {
            return _queries.Remove(key);
        }

        /// <summary>
        /// Получает значение, связанное с указанным ключом.
        /// </summary>
        /// <returns>
        /// Значение true, если объект, реализующий <see cref="T:System.Collections.Generic.IDictionary`2"/>, содержит элемент с указанным ключом, в противном случае — значение false.
        /// </returns>
        /// <param name="key">Ключ, значение которого необходимо получить.</param><param name="value">Этот метод возвращает значение, связанное с указанным ключом, если он найден; в противном случае — значение по умолчанию для данного типа параметра <paramref name="value"/>. Этот параметр передается неинициализированным.</param><exception cref="T:System.ArgumentNullException">Параметр <paramref name="key"/> имеет значение null.</exception>
        public bool TryGetValue(string key, out string value)
        {
            if (ContainsKey(key))
            {
                value = this[key];
                return true;
            }

            value = null;
            return false;
        }

        /// <summary>
        /// Возвращает или задает элемент с указанным ключом.
        /// </summary>
        /// <returns>
        /// Элемент с указанным ключом.
        /// </returns>
        /// <param name="key">Ключ элемента, который требуется получить или задать.</param><exception cref="T:System.ArgumentNullException">Параметр <paramref name="key"/> имеет значение null.</exception><exception cref="T:System.Collections.Generic.KeyNotFoundException">Свойство получено и параметр <paramref name="key"/> не найден.</exception><exception cref="T:System.NotSupportedException">Свойство задано, и объект <see cref="T:System.Collections.Generic.IDictionary`2"/> доступен только для чтения.</exception>
        public string this[string key]
        {
            get { return _queries[key]; }
            set { _queries[key] = value; }
        }

        /// <summary>
        /// Получает интерфейс <see cref="T:System.Collections.Generic.ICollection`1"/>, содержащий ключи <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// Интерфейс <see cref="T:System.Collections.Generic.ICollection`1"/>, содержащий ключи объекта, который реализует <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        public ICollection<string> Keys
        {
            get { return _queries.Keys; }
        }

        /// <summary>
        /// Получает коллекцию <see cref="T:System.Collections.Generic.ICollection`1"/>, содержащую значения из словаря <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// Интерфейс <see cref="T:System.Collections.Generic.ICollection`1"/>, содержащий значения объекта, который реализует <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        public ICollection<string> Values
        {
            get { return _queries.Values; }
        }

        /// <summary>
        /// Играет роль хэш-функции для определенного типа.
        /// </summary>
        /// <returns>
        /// Хэш-код для текущего объекта <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return Query.GetHashCode();
        }

        /// <summary>
        /// Определяет, равен ли заданный объект <see cref="T:System.Object"/> текущему объекту <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true, если заданный объект равен текущему объекту; в противном случае — false.
        /// </returns>
        /// <param name="obj">Объект, который требуется сравнить с текущим объектом.</param>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var query = Query;

            return query.Equals(((QueryCollection) obj).Query);
        }
    }
}