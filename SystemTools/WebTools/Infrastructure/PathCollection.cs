using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SystemTools.WebTools.Infrastructure
{
    internal class PathCollection : ICollection<string>
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
            {
                if (string.IsNullOrEmpty(path[0]))
                    return;

                Paths.Add(path[0]);
            }
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

        public string Path
        {
            get
            {
                var s = string.Empty;
                return _paths.Aggregate(s, (s1, next) => string.Format("{0}/{1}", s1, next), s1 => s1 + "/");
//                return _paths.Aggregate((current, next) => string.Format("{0}/{1}", current, next));
            }
            set
            {
                _paths.Clear();
                Add(value);
            }
        }

        /// <summary>
        /// Играет роль хэш-функции для определенного типа.
        /// </summary>
        /// <returns>
        /// Хэш-код для текущего объекта <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return Path.GetHashCode();
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

            var query = Path;

            return query.Equals(((PathCollection)obj).Path);
        }
    }
}