using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Mvc;

namespace TaxOrg.Infrastructure
{
    [ModelBinder(typeof(GridModelBinder))]
    public class GridSettings
    {
        private int _pageIndex = 1;

        /// <summary>
        /// Признак активации поиска
        /// </summary>
        public bool IsSearch { get; set; }
        /// <summary>
        /// Количество страниц
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Номер запрашиваемой страницы
        /// </summary>
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }

        /// <summary>
        /// Столбец, по которому будет проводиться сортировка
        /// </summary>
        public string SortColumn { get; set; }
        /// <summary>
        /// Направление сортировки
        /// </summary>
        public string SortOrder { get; set; }
        /// <summary>
        /// Объект фильтра который задал пользователь
        /// </summary>
        public Filter Where { get; set; }
    }

    [DataContract]
    public class Filter
    {
        /// <summary>
        /// Оператор применяемый к группе правил Rules
        /// </summary>
        [DataMember]
        public string groupOp { get; set; }

        /// <summary>
        /// Набор правил фильтрации
        /// </summary>
        [DataMember]
        public Rule[] rules { get; set; }

        /// <summary>
        /// Создание объекта Filter
        /// </summary>
        /// <param name="jsonData">Строка запроса параметров фильтра в формате JSON</param>
        /// <returns>Возвращает сериализованный объект фильтра</returns>
        public static Filter Create(string jsonData)
        {
            try
            {
                //byte[] bs = Encoding.Convert(Encoding.GetEncoding(jsonData), Encoding.UTF8,
                //                             Encoding.Unicode.GetBytes(jsonData));
                var serializer = new DataContractJsonSerializer(typeof (Filter));
                //var ms = new MemoryStream(Encoding.Default.GetBytes(jsonData));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonData));
                return serializer.ReadObject(ms) as Filter;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    [DataContract]
    public class Rule   
    {
        /// <summary>
        /// Поле, к которому применяется правило
        /// </summary>
        [DataMember]
        public string field { get; set; }

        /// <summary>
        /// Операция, которую выбрал пользователь
        /// </summary>
        [DataMember]
        public string op { get; set; }

        /// <summary>
        /// Фильтр, который ввел пользователь
        /// </summary>
        [DataMember]
        public string data { get; set; }
    }
}