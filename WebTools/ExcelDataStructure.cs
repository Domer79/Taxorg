namespace SystemTools
{
//    public class ExcelDataStructure
//    {
////        public static T GetThisObject<T>(ExcelRow<T> excelRow) 
////            where T : ExcelDataStructure, new()
////        {
////            var @object = new T();
////            foreach (var propertyInfo in typeof(T).GetProperties())
////            {
////                var attr = (ExcelColumnAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof (ExcelColumnAttribute));
////
////                propertyInfo.SetValue(@object, Convert.ChangeType(excelRow[attr.Order], propertyInfo.PropertyType));
////            }
////
////            return @object;
////        }
//    }
}