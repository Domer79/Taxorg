//using JavaPlagins.Attributes;
//using JavaPlagins.BlJqGrid;
//
//namespace TaxorgRepository.Interfaces
//{
//    public interface IOrganization
//    {
//        [JqGridColumn(PrimaryKey = true)]
//        int IdOrganization { get; }
//
//        [JqGridColumn(Caption = "Полное название организации", EditType = EditType.TextBox, Sortable = true)]
//        string Name { get; set; }
//
//        [JqGridColumn(Caption = "Сокращенное название организации", EditType = EditType.TextBox, Sortable = true)]
//        string ShortName { get; set; }
//
//        [JqGridColumn(DataField = "addr", Caption = "Почтовый адрес", EditType = EditType.TextBox)]
//        string Address { get; set; }
//
//        [JqGridColumn(Caption = "ИНН", EditType = EditType.TextBox)]
//        string Inn { get; set; }
//    }
//}