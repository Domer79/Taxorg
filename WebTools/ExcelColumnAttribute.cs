using System;

namespace WebTools
{
    public class ExcelColumnAttribute : Attribute
    {
        private readonly int _order;
        private readonly string _columnName;

        public ExcelColumnAttribute(int order, string columnName = null)
        {
            _order = order;
            _columnName = columnName;
        }

        public int Order
        {
            get { return _order; }
        }

        public string ColumnName
        {
            get { return _columnName; }
        }
    }
}
