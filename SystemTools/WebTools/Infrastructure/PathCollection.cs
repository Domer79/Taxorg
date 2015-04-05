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
        /// �������������� ����� ��������� ������ <see cref="T:System.Object"/>.
        /// </summary>
        public PathCollection()
        {
        }

        public PathCollection(string paths)
        {
            Add(paths);
        }

        /// <summary>
        /// ���������� �������������, ����������� �������� � ���������.
        /// </summary>
        /// <returns>
        /// ��������� <see cref="T:System.Collections.Generic.IEnumerator`1"/>, ������� ����� �������������� ��� �������� ��������� ���������.
        /// </returns>
        public IEnumerator<string> GetEnumerator()
        {
            return Paths.GetEnumerator();
        }

        /// <summary>
        /// ���������� �������������, �������������� �������� � ���������.
        /// </summary>
        /// <returns>
        /// ������ <see cref="T:System.Collections.IEnumerator"/>, ������� ����� �������������� ��� �������� ���������.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// ��������� ������� � ��������� <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">������, ����������� � ��������� <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">������ <see cref="T:System.Collections.Generic.ICollection`1"/> �������� ������ ��� ������.</exception>
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
        /// ������� ��� �������� �� ��������� <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">������ <see cref="T:System.Collections.Generic.ICollection`1"/> �������� ������ ��� ������.</exception>
        public void Clear()
        {
            Paths.Clear();
        }

        /// <summary>
        /// ����������, �������� �� ��������� <see cref="T:System.Collections.Generic.ICollection`1"/> ��������� ��������.
        /// </summary>
        /// <returns>
        /// �������� true, ���� �������� <paramref name="item"/> ������ � ��������� <see cref="T:System.Collections.Generic.ICollection`1"/>; � ��������� ������ � �������� false.
        /// </returns>
        /// <param name="item">������, ������� ��������� ����� � <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
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
        /// �������� �������� <see cref="T:System.Collections.Generic.ICollection`1"/> � ������ <see cref="T:System.Array"/>, ������� � ���������� ������� <see cref="T:System.Array"/>.
        /// </summary>
        /// <param name="array">���������� ������ <see cref="T:System.Array"/>, � ������� ���������� �������� �� ���������� <see cref="T:System.Collections.Generic.ICollection`1"/>. ������ <see cref="T:System.Array"/> ������ ����� ����������, ������������ � ����.</param><param name="arrayIndex">������������� �� ���� ������ � ������� <paramref name="array"/>, ����������� ������ �����������.</param><exception cref="T:System.ArgumentNullException">�������� <paramref name="array"/> ����� �������� null.</exception><exception cref="T:System.ArgumentOutOfRangeException">�������� ��������� <paramref name="arrayIndex"/> ������ 0.</exception><exception cref="T:System.ArgumentException">���������� ��������� � �������� ��������� <see cref="T:System.Collections.Generic.ICollection`1"/> ��������� ��������� �����, ������� � ������� <paramref name="arrayIndex"/> �� ����� ������� ���������� <paramref name="array"/>.</exception>
        public void CopyTo(string[] array, int arrayIndex)
        {
            Paths.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// ������� ������ ��������� ���������� ������� �� ��������� <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// �������� true, ���� ������� <paramref name="item"/> ������� ������ �� <see cref="T:System.Collections.Generic.ICollection`1"/>, � ��������� ������ � �������� false. ���� ����� ����� ���������� �������� false, ���� �������� <paramref name="item"/> �� ������ � �������� ���������� <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">������, ������� ���������� ������� �� ��������� <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">������ <see cref="T:System.Collections.Generic.ICollection`1"/> �������� ������ ��� ������.</exception>
        public bool Remove(string item)
        {
            var paths = SplitPath(item);
            return paths.All(path => Paths.Remove(path));
        }

        /// <summary>
        /// �������� ����� ���������, ������������ � ���������� <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// ����� ���������, ������������ � ���������� <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count
        {
            get { return Paths.Count; }
        }

        /// <summary>
        /// �������� ��������, �����������, �������� �� <see cref="T:System.Collections.Generic.ICollection`1"/> ������ ��� ������.
        /// </summary>
        /// <returns>
        /// �������� true, ���� <see cref="T:System.Collections.Generic.ICollection`1"/> �������� ������ ��� ������; � ��������� ������ � �������� false.
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
        /// ������ ���� ���-������� ��� ������������� ����.
        /// </summary>
        /// <returns>
        /// ���-��� ��� �������� ������� <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }

        /// <summary>
        /// ����������, ����� �� �������� ������ <see cref="T:System.Object"/> �������� ������� <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true, ���� �������� ������ ����� �������� �������; � ��������� �����堗 false.
        /// </returns>
        /// <param name="obj">������, ������� ��������� �������� � ������� ��������.</param>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var query = Path;

            return query.Equals(((PathCollection)obj).Path);
        }
    }
}