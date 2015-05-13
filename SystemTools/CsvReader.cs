using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using SystemTools.Interfaces;

namespace SystemTools
{
    public delegate void CsvReaderErrorHandler(Exception e, string bugRow);
    public class CsvReader<T> : IEnumerable<T>, IDisposable where T : class
    {
        private HashSet<IRow<T>> _hash;
        private readonly FileStream _fileStream;
        private readonly CsvReaderErrorHandler _errorHandler;

        public CsvReader(string fileName, CsvReaderErrorHandler errorHandler = null)
            : this(new FileStream(fileName, FileMode.Open), errorHandler)
        {
        }

        public CsvReader(FileStream fileStream, CsvReaderErrorHandler errorHandler = null)
        {
            if (fileStream == null) 
                throw new ArgumentNullException("fileStream");

            _fileStream = fileStream;

            _errorHandler = errorHandler ?? ((exception, row) => Console.WriteLine("Bug string: {0}, message: {1}", row, exception.Message));
            FillHashSet();
        }

        public HashSet<IRow<T>> CsvHash
        {
            get
            {
                if (_hash == null)
                    FillHashSet();
                return _hash;
            }
        }

        private void FillHashSet()
        {
            _hash = new HashSet<IRow<T>>();
            var sr = new StreamReader(_fileStream, Encoding.GetEncoding(866), false);

            var buffer = new byte[sr.BaseStream.Length];
            sr.BaseStream.Read(buffer, 0, buffer.Length);
            var encodingString = Encoding.GetEncoding(1251).GetString(buffer);
            using (var str = new StringReader(encodingString))
            {
                str.ReadLine();
                var s = str.ReadLine();
                do
                {
                    _hash.Add(new CsvRow<T>(s));
                    s = str.ReadLine();
                } while (s != null);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return CsvHash.Select(row => row.GetObject(_errorHandler)).Where(obj => obj != null).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Close()
        {
            _fileStream.Flush();
            _fileStream.Close();
        }

        public void Dispose()
        {
            Close();
        }
    }

    public class CsvRow<T> : IRow<T> where T : class
    {
        private readonly string _strRow;
        private string[] _row;

        public CsvRow(string strRow)
        {
            _strRow = strRow;
        }

        private string[] Row
        {
            get { return _row = (_row = Split2()); }
        }

        private string[] Split()
        {
            try
            {
                return _strRow.Split(new[] {';'});
            }
            catch (IndexOutOfRangeException)
            {
                return _strRow.Split(new[] { ',' });
            }
        }

#if DEBUG
        private string _splited2Row;
#endif

        private string[] Split2()
        {
            var rx = new Regex(@"^(?<period>\d{2}\.\d{2}\.\d{4});(?<inn>\d{10});(?<kbk>\d{17,20});(?<kbkname>.+);(?<balance>.+)");
            var match = rx.Match(_strRow);

            if (!match.Success)
                throw new InvalidOperationException("CsvRow. Входная строка имеет неверный формат");

            var result = new[]
            {
                match.Groups["period"].Value, 
                match.Groups["inn"].Value, 
                match.Groups["kbk"].Value,
                match.Groups["kbkname"].Value, 
                match.Groups["balance"].Value
            };

#if DEBUG
            _splited2Row = result.Aggregate((c, n) => c + "|" + n);
#endif

            return result;
        }

        public T GetObject(CsvReaderErrorHandler errorHandler)
        {
            try
            {
                var @object = Activator.CreateInstance(typeof(T));
                foreach (var propertyInfo in @object.GetType().GetProperties().Where(pi => Attribute.IsDefined(pi, typeof(ExcelColumnAttribute))))
                {
                    var attr = (ExcelColumnAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(ExcelColumnAttribute));

                    if (attr == null)
                        throw new InvalidOperationException(string.Format("Произошла ошибка при попытке считать атрибут у свойства {0}", propertyInfo.Name));

                    propertyInfo.SetValue(@object, Convert.ChangeType(Row[attr.Order - 1].Trim(), propertyInfo.PropertyType));
                }

                return (T)@object;
            }
            catch (Exception e)
            {
                //TODO  Buges.SaveBugRow(_strRow, e.Message); ErrorLog.SaveError(e);
                if (errorHandler != null)
                    errorHandler(e, _strRow);
                return null;
            }
        }

        public T GetObject()
        {
            return GetObject(null);
        }
    }
}