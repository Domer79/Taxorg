//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.IO.Compression;
//using System.Reflection;
//using Microsoft.Office.Interop.Excel;
//
//namespace TaxOrg.Tools
//{
//	/// <summary>
//	/// Утилиты для работы с файлами
//	/// </summary>
//	public static class FileUtils
//	{
//		#region Поиск свободного имени файла для создания такого на диске
//
//		/// <summary>
//		/// Получить имя свободного файла для создания на диске
//		/// </summary>
//		/// <param name="defPref">Префикс для файла по умолчанию</param>
//		/// <param name="fileName">Имя файл</param>
//		/// <returns></returns>
//		public static string GetFreeFileName( string defPref, string fileName )
//		{
//			int cnt = 0;
//			while (!ResolveName( defPref + cnt + fileName ))
//			{
//				cnt++;
//			}
//			return defPref + cnt + fileName;
//		}
//
//
//		/// <summary>
//		/// Проверка на возможность использования этого имени файла для создания на диске
//		/// </summary>
//		/// <returns>TRUE - если это имя можно использовать</returns>
//		private static bool ResolveName( string tmpFileName )
//		{
//			if (!File.Exists( tmpFileName ))
//			{
//                return true;
//			}
//
//            try
//            {
//                File.Delete(tmpFileName);
//                return !File.Exists(tmpFileName);
//            }
//            catch (System.IO.IOException)
//            { }
//            catch
//            { }
//
//			return false;
//		}
//
//		#endregion
//
//		/// <summary>
//		/// Очистка временых файлов каталоге по заданному префиксу
//		/// </summary>
//		/// <param name="defPref"></param>
//		public static void ClearTemporaryFiles( string defPref )
//		{
//			if (defPref == null || defPref == "")
//				return;
//
//			string[] tmpFiles = Directory.GetFiles( AppDomain.CurrentDomain.BaseDirectory, defPref + "*" );
//
//			if (tmpFiles.Length <= 0)
//				return;
//			foreach (string file in tmpFiles)
//			{
//				try
//				{
//					File.Delete( file );
//				}
//				catch
//				{}
//			}
//		}
//
//	    public static bool Exists( string fileName )
//	    {
//	        return File.Exists( fileName );
//	    }
//
//        public static string GetSettingsStoringFolder()
//        {
//            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Assembly.GetEntryAssembly().GetName().Name);
//        }
//
//	    #region From UKV MP
//
//	    public static void ClearTemporaryFiles(string defPref, string[] fileExtensions)
//	    {
//	        ClearTemporaryFiles(AppDomain.CurrentDomain.BaseDirectory, defPref, fileExtensions);
//	    }
//
//	    public static void ClearTemporaryFiles(string filePath, string defPref, string[] fileExtensions)
//	    {
//	        if (String.IsNullOrEmpty(defPref))
//	            return;
//
//	        var tmpFiles = new List<string>();
//
//	        foreach (string fileExtension in fileExtensions)
//	        {
//	            tmpFiles.AddRange(Directory.GetFiles(filePath, defPref + "*." + fileExtension));
//	        }
//	        //             tmpFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, defPref + "*." );
//
//	        if (tmpFiles.Count <= 0)
//	            return;
//
//	        foreach (string file in tmpFiles)
//	        {
//	            try
//	            {
//	                File.Delete(file);
//	            }
//	            catch
//	            {
//	            }
//	        }
//	    }
//
//        public static string GetFreeFileName(string defPref, string fileName, string fileExtention, string filePath)
//        {
//            if (ResolveName(filePath + defPref + fileName + fileExtention))
//            {
//                return filePath + defPref + fileName + fileExtention;
//            }
//
//            int cnt = 1;
//            while (!ResolveName(filePath + defPref + fileName + cnt + fileExtention))
//            {
//                cnt++;
//            }
//
//            return filePath + defPref + fileName + cnt + fileExtention;
//        }
//
//
//	    #endregion
//
//	    #region FS
//        public const int GZipDecompressBufferSize = 8232960;
//
//        public static void CompressFile(string sourceFile, string targetFile)
//        {
//            ExtCompressFile(new FileInfo(sourceFile), targetFile);
//        }
//
//
//
//        public static void ExtCompressFile(this FileInfo sourceFile, string targetFile)
//        {
//            using (FileStream fileStream = File.OpenRead(sourceFile.FullName))
//            using (var compressedStream = new FileStream(targetFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
//            using (Stream zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
//            {
//                var buffer = new byte[8040]; //chunk sizes that are multiples of 8040 bytes.
//                int read;
//
//                while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
//                {
//                    zipStream.Write(buffer, 0, read);
//                }
//
//                zipStream.Close();
//            }
//        }
//
//	    public static void Decompress(string sourceFile, string targetFile)
//	    {
//	        using (var compressedStream = new FileStream(sourceFile, FileMode.OpenOrCreate, FileAccess.Read))
//	        using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
//	        using (var decompressedStream = new FileStream(targetFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
//	        {
//	            var buffer = new byte[GZipDecompressBufferSize];
//	            int read;
//
//	            while ((read = zipStream.Read(buffer, 0, buffer.Length)) > 0)
//	            {
//	                decompressedStream.Write(buffer, 0, read);
//	            }
//	        }
//	    }
//
//
//	    public static byte[] ExtGetOptimalBufferSize(this FileInfo currentUploadingFile)
//	    {
//	        return ExtGetOptimalBufferSize(currentUploadingFile.Length);
//	    }
//
//	    public static byte[] ExtGetOptimalBufferSize(long fileLength)
//	    {
//	        const int chunkBufferSize = 8040;
//	        byte[] buffer;
//
//	        if (fileLength >= chunkBufferSize * 1024)
//	        {
//	            buffer = new byte[chunkBufferSize * 1024];
//	        }
//
//	        else if (fileLength >= chunkBufferSize * 100)
//	        {
//	            buffer = new byte[chunkBufferSize * 100];
//	        }
//
//	        else if (fileLength >= chunkBufferSize * 25)
//	        {
//	            buffer = new byte[chunkBufferSize * 25];
//	        }
//
//	        else
//	        {
//	            buffer = new byte[fileLength];
//	        }
//	        return buffer;
//	    }
//
//	    #endregion
//
//	    #region Check File Using
//	    private static bool IsFileInUse(string filename)
//	    {
//	        try
//	        {
//	            using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
//	            {
//	                return false;
//	            }
//	        }
//	        catch (IOException)
//	        {
//	            return true;
//	        }
//	    }
//
//	    #endregion
//
//	}
//}

namespace WebTools
{
}