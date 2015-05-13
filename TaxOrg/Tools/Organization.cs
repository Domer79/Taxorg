using System;
using SystemTools;

namespace TaxOrg.Tools
{
    public class Organization
    {
        [ExcelColumn(1, "����")]
        public DateTime Date { get; set; }

        [ExcelColumn(2, "���")]
        public string Inn { get; set; }

        [ExcelColumn(3, "���")]
        public string TaxCode { get; set; }

        [ExcelColumn(4, "������������ ���")]
        public string TaxName { get; set; }

        [ExcelColumn(5, "����� �����")]
        public decimal Tax { get; set; }

        public override string ToString()
        {
            return string.Format("����: {0}, ���: {1}, ���: {2}, ����� �����: {3}.", Date, Inn, TaxCode, Tax);
        }
    }
}