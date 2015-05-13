using System;
using SystemTools;

namespace TaxOrg.Tools
{
    public class Organization
    {
        [ExcelColumn(1, "Дата")]
        public DateTime Date { get; set; }

        [ExcelColumn(2, "ИНН")]
        public string Inn { get; set; }

        [ExcelColumn(3, "КБК")]
        public string TaxCode { get; set; }

        [ExcelColumn(4, "Наименование КБК")]
        public string TaxName { get; set; }

        [ExcelColumn(5, "Сумма долга")]
        public decimal Tax { get; set; }

        public override string ToString()
        {
            return string.Format("Дата: {0}, ИНН: {1}, КБК: {2}, Сумма долга: {3}.", Date, Inn, TaxCode, Tax);
        }
    }
}