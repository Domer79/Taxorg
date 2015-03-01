//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Runtime.InteropServices;
//using System.Text.RegularExpressions;
//using Microsoft.Office.Core;
//using Microsoft.Office.Interop.Excel;
//using TaxOrg.Interfaces;
//
//namespace TaxOrg.Tools
//{
//	/// <summary>
//	/// Упрощает работу с листом Excel.
//	/// </summary>
//	public sealed class ExcelSheet : IDisposable
//	{
//		private _Application _excel;
//		private Workbook _book;
//		private Worksheet _sheet;
//
//		public static readonly Object Missing = Type.Missing;
//	    private static int _disposedCols = 4;
//	    private static int _disposedRows = 4;
//
//	    public Worksheet Sheet
//		{
//			get { return _sheet; }
//			set { _sheet = value; }
//		}
//
//	    public Workbook Book
//	    {
//	        get { return _book; }
//	    }
//
//	    public ExcelSheet()
//		{
////			Missing = Type.Missing;
////			_excel = new ApplicationClass();
//			_excel = new Application();
////	        _excel.Visible = true;
////	        _excel.DisplayAlerts = false;
//		}
//
//        public ExcelSheet(string fileName)
//            :this()
//        {
//            Open(fileName);
//        }
//
//        public ExcelSheet(Worksheet sheet)
//        {
//            _sheet = sheet;
//        }
//
//	    public _Application Excel
//	    {
//	        get { return _excel; }
//	        set { _excel = value; }
//	    }
//
//
//	    public void SetValue( Int32 rowNo, Int32 colNo, Object value )
//		{
//			Sheet.Cells[ rowNo, colNo ] = value;
//		}
//
//		public string this[ Int32 rowNo, Int32 colNo ]
//		{
//			get { return ((Range)Sheet.Cells[ rowNo, colNo ]).Text.ToString(); }
//			set { Sheet.Cells[ rowNo, colNo ] = value; }
//		}
//
//		public void CloneRow( Int32 templateRowNo, Int32 times )
//		{
//			for (Int32 i = 0; i < times; ++i)
//			{
//				Range row = (Range)Sheet.Rows[ templateRowNo + ":" + templateRowNo, Missing ];
//				row.Select();
//				row.Copy( Missing );
//				row.Insert( XlInsertShiftDirection.xlShiftDown, true );
//			}
//		}
//
//		public void Open( String filePath )
//		{
//			_book = _excel.Workbooks.Open( Path.GetFullPath( filePath ), false, true, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing );
//			Sheet = (Worksheet)Book.Worksheets[ 1 ];
//		}
//
//		public void SaveAs( String filePath )
//		{
//		    PropertyInfo property = typeof (Workbook).GetProperty("CheckCompatibility");
//            if(property != null)
//		        Book.CheckCompatibility = false;
//
//            _excel.DisplayAlerts = false;
//
////		    Book.SaveAs(filePath, GetFormatByExtension(filePath), Missing, Missing, Missing, Missing,
//            Book.SaveAs(filePath,Regex.Replace(filePath, @".*\.", "").Equals("xlsx")
//                                  ? XlFileFormat.xlOpenXMLWorkbook
//                                 : XlFileFormat.xlWorkbookNormal, Missing, Missing, Missing, Missing,
//		                      XlSaveAsAccessMode.xlExclusive, Missing, Missing, Missing, Missing, Missing);
//		}
//
////	    private XlFileFormat GetFormatByExtension(string filePath)
////	    {
////	        XlFileFormat format;
////	        string ext = Regex.Replace(filePath, @".*\.", "");
////
////	        if (ext.Equals("xlsx"))
////	            format = XlFileFormat.xlOpenXMLWorkbook;
////	        else if (ext.Equals("xltm"))
////	            format = XlFileFormat.xlOpenXMLTemplateMacroEnabled;
////	        else
////	            format = XlFileFormat.xlWorkbookNormal;
////
////	        return format;
////	    }
//
//	    public void SetDisplayAlerts(bool value)
//        {
//            Book.Application.DisplayAlerts = value;}
//
//		public void Show()
//		{
//			_excel.Visible = true;
//		}
//
//		public void Release()
//		{
////            Marshal.ReleaseComObject(_sheet);
////            Marshal.ReleaseComObject(_book);
////            Marshal.ReleaseComObject(_excel);
//
//
//			Sheet = null;
//
//			if (Book != null)
//			{
//				Book.Close( false, Missing, Missing );
//				_book = null;
//			}
//
//			if (_excel != null)
//			{
//				_excel.Quit();
//				_excel = null;
//			}
//		}
//
//		public void Release( bool collectGarbage )
//		{
//			Release();
//
//			if (collectGarbage)
//				GC.Collect();
//		}
//
//		public void ReleaseReference()
//		{
//            try
//            {
//                _excel.Quit();
//                Marshal.ReleaseComObject(_excel);
//                _excel = null;
//            }
//            catch (Exception e)
//            {
//                _excel = null;
//                Console.Write("При освобождении объекта произошло исключение " + e);
//            }
//            finally
//            {
//                GC.Collect();
//            }
//		}
//
//		//---------------------------------------------------------------
//		/// <summary>
//		/// Активация листа
//		/// </summary>
//		public void ActivateSheet( int sheetNumber )
//		{
//            if (Book == null)
//                _book = _excel.Workbooks.Add(Type.Missing);
//            else
//            {
//                Sheet = (Worksheet)Book.Worksheets[sheetNumber];
//                //			Sheet.Activate();
//                ((Worksheet)Book.Worksheets[sheetNumber]).Activate();
//            }
//		}
//
//		/// <summary>
//		/// Запуск макроса
//		/// </summary>
//		/// <param name="oRunArgs"></param>
//		public void RunMacro( object[] oRunArgs )
//		{
//			try
//			{
//				_excel.GetType().InvokeMember("Run",
//				                              BindingFlags.Default |
//				                              	BindingFlags.InvokeMethod,
//				                              null, _excel, oRunArgs);
//			}
//			catch (Exception ex)
//			{
//                DebugHelper.Write(ex);
//			}
//		}
//
//
//        public string GetDateFormat()
//        {
//            int ver = _excel.LanguageSettings.LanguageID[MsoAppLanguageID.msoLanguageIDUI];
//            if (ver == 1049)
//                return "ДД.ММ.ГГ";
//            else
//                return "DD.MM.YY";
//        }
//
//	    public int CreateCopy()
//	    {
//            (( Worksheet )_excel.ActiveSheet).Copy ( Type.Missing, _excel.Sheets[_excel.Sheets.Count] );
//	        return _excel.Sheets.Count;
//	    }
//
//	    public void SetActiveSheetName( string sheetName )
//	    {
//            string preparingNAme = sheetName;
//            if( preparingNAme.Length >= 31 )
//                preparingNAme = preparingNAme.Substring( 0, 30 );
//            foreach (char chr in @"/\[]*?:;")
//	        {
//                preparingNAme = preparingNAme.Replace(chr, '.');
//	        }
//            preparingNAme = GenerateSheetNameWithNumber ( preparingNAme, 0 );
//            (( Worksheet )_excel.ActiveSheet).Name = preparingNAme;
//	    }
//
//	    private string GenerateSheetNameWithNumber( string sheetName, int iterateNumber )
//	    {
//	        string res = (iterateNumber == 0) ? sheetName : sheetName + iterateNumber;
//            for( int i = 1; i < _excel.Sheets.Count; i++ )
//            {
//                if( (( Worksheet )_excel.Sheets[i]).Name == res )
//                    return GenerateSheetNameWithNumber( sheetName, iterateNumber + 1 );
//            }
//
//	        return res;
//	    }
//
//	    public object this[string cellName]
//	    {
//            get { return Sheet.get_Range(cellName, Type.Missing).Value2; }
//            set { Sheet.get_Range(cellName, Type.Missing).Value2 = value; }
//	    }
//
//        public void Save()
//        {
//            Book.Save();
//        }
//
//	    public void SelectFirstCell()
//	    {
//            var range = Sheet.Range["A1", "A1"];
//            range.Select();
//	    }
//
//	    public void Dispose()
//	    {
////	        _excel.Quit();
//            ReleaseReference();
//	    }
//
//	    public void InsertRow(int rowIndex)
//	    {
//	        var row = (Range)Sheet.Rows[rowIndex, Missing];
//	        row.Insert();
//	    }
//
//	    public static int DisposedCols
//	    {
//	        get { return _disposedCols; }
//	        set { _disposedCols = value; }
//	    }
//
//	    public static int DisposedRows
//	    {
//	        get { return _disposedRows; }
//	        set { _disposedRows = value; }
//	    }
//	}
//
//    public class ExcelData<T> : IEnumerable<ExcelRow<T>>, IDisposable
//        where T : ExcelDataStructure, new()
//    {
//        private readonly ExcelSheet _sheet;
//
//        public ExcelData(string fileName)
//            : this(new ExcelSheet(fileName))
//        {
//        }
//
//        public ExcelData(ExcelSheet sheet)
//        {
//            _sheet = sheet;
//        }
//
//        public IEnumerator<ExcelRow<T>> GetEnumerator()
//        {
//            return new ExcelRowsIEnumerator(_sheet, ExcelSheet.DisposedRows, ExcelSheet.DisposedCols);
//        }
//
//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return GetEnumerator();
//        }
//
//        public class ExcelRowsIEnumerator : IEnumerator<ExcelRow<T>>
//        {
//            private readonly ExcelSheet _sheet;
//            private readonly int _disposedRows;
//            private readonly int _disposedCols;
//            private readonly HashSet<ExcelRow<T>> _hash = new HashSet<ExcelRow<T>>();
//            private int _position = 0;
//
//            public ExcelRowsIEnumerator(ExcelSheet sheet, int disposedRows, int disposedCols)
//            {
//                if (sheet == null) 
//                    throw new ArgumentNullException("sheet");
//
//                _sheet = sheet;
//                _disposedRows = disposedRows;
//                _disposedCols = disposedCols;
//            }
//
//            public void Dispose()
//            {
//                _hash.Clear();
//            }
//
//            public bool MoveNext()
//            {
//                var pos = _position+1;
//
//                for (var i = pos; i <= pos+_disposedRows; i++)
//                {
//                    for (int j = 1; j <= _disposedCols; j++)
//                    {
//                        if (string.IsNullOrEmpty(_sheet[pos, j]))
//                            continue;
//
//                        _position++;
//                        return true;
//                    }
////                    pos++;
//                }
//                return false;
//            }
//
//            public void Reset()
//            {
//                _position = -1;
//            }
//
//            public ExcelRow<T> Current
//            {
//                get
//                {
//                    var row = _hash.FirstOrDefault(v => v.RowIndex == _position);
//                    if (row == null)
//                        _hash.Add(new ExcelRow<T>(_sheet, _position));
//
//                    return _hash.FirstOrDefault(v => v.RowIndex == _position);
//                }
//            }
//
//            object IEnumerator.Current
//            {
//                get { return Current; }
//            }
//        }
//
//        public void Dispose()
//        {
//            _sheet.Dispose();
//        }
//    }
//
//    public class ExcelRow<T> : IRow<T> where T : ExcelDataStructure, new()
//    {
//        private readonly ExcelSheet _sheet;
//        private readonly int _rowIndex;
//
//        internal ExcelRow(ExcelSheet sheet, int rowIndex)
//        {
//            if (sheet == null)
//                throw new ArgumentNullException("sheet");
//
//            _sheet = sheet;
//            _rowIndex = rowIndex;
//        }
//
//        public int RowIndex
//        {
//            get { return _rowIndex; }
//        }
//
//        public override int GetHashCode()
//        {
//            return RowIndex.GetHashCode();
//        }
//
//        public override bool Equals(object obj)
//        {
//            if (obj as ExcelRow<T> == null)
//                return false;
//
//            return RowIndex == ((ExcelRow<T>) obj).RowIndex;
//        }
//
//        public T GetObject()
//        {
//            return ExcelDataStructure.GetThisObject(this);
//        }
//
//        public object this[int order]
//        {
//            get { return _sheet[RowIndex, order]; }
//        }
//    }
//}

namespace SystemTools
{
}