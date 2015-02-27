using JavaPlagins.Attributes;
using JavaPlagins.BlJqGrid;

namespace TaxorgRepository.Models
{
    public class OrganizationBak
    {
        [JqGridColumn(PrimaryKey = true)]
        public int IdOrganization { get; private set; }

        [JqGridColumn(Caption = "Полное название организации", EditType = EditType.TextBox,
            Sortable = true)]
        public string Name { get; set; }

        [JqGridColumn(Caption = "Сокращенное название организации", EditType = EditType.TextBox,
            Sortable = true)]
        public string ShortName { get; set; }

        [JqGridColumn(DataField = "addr", Caption = "Почтовый адрес", EditType = EditType.TextBox)]
        public string Address { get; set; }

        [JqGridColumn(Caption = "ИНН", EditType = EditType.TextBox)]
        public string Inn { get; set; }
    }
}
