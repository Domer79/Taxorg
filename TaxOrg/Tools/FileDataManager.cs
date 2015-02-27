using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Web.Mvc;
using TaxorgRepository.Models;
using TaxorgRepository.Repositories;
using TaxorgRepository.Tools;

namespace TaxOrg.Tools
{
    internal class FileDataManager
    {
        #region Service

        #region Save

        public static FSFileInstance SaveFile(string fileName)
        {
            return SaveFile(new FileInfo(fileName));
        }

        public static FSFileInstance SaveFile(FileInfo fileInfo)
        {
            FSFileInstance fileInstance = new FSFileInstance(null);
            SaveFile(fileInstance, fileInfo);
            return fileInstance;
        }

        public static void SaveFile(FSFileInstance fileInstance, string fileName)
        {
            SaveFile(fileInstance, new FileInfo(fileName));
        }

        public static FSFileInstance SaveFile(Stream stream, int? idFile, string fileName, string ext)
        {
            var fileInstance = new FSFileInstance(idFile <= 0 ? null : idFile);
            fileInstance.FileName = fileName;
            fileInstance.FileExtension = ext;
            SaveFile(fileInstance, stream, true);

            return fileInstance;
        }

        public static void SaveFile(FSFileInstance fileInstance, FileInfo fileInfo)
        {
            fileInstance.FileName = fileInfo.Name;
            fileInstance.FileExtension = fileInfo.Extension;

            ReportImportStatus("Сжатие и сохранение файла на локальной дисковой системе во временной папке");
            var compressedFileName = SaveCompressedFile(fileInfo);
            var fileName = compressedFileName;
            fileInstance.IsCompressed = fileInfo.Length > new FileInfo(compressedFileName).Length;

            if (!fileInstance.IsCompressed)
                fileName = fileInfo.FullName;

            try
            {
                using (var fileStream = File.OpenRead(fileName))
                {
                    SaveFile(fileInstance, fileStream);
                }
            }
            finally
            {
                try
                {
                    File.Delete(compressedFileName);
                }
                catch (Exception e)
                {
                    throw new Exception("Ошибка удаления временного файла: " + e.Message, e);
                }

            }
        }

        private static void SaveFile(FileSystem fileInstance, Stream stream, bool compress = false)
        {
            fileInstance.FileAuthor = UserWindowsIdentity.Instance.GetUserWithInitials();
            fileInstance.FileDateAdd = DateTime.Now;

            fileInstance.PrepareRow();
            try
            {
                using (var ms = new MemoryStream())
                {
                    var saveStream = stream;
                    if (compress)
                    {
                        ReportImportStatus("Сжатие данных");
                        stream.Compress(ms);

                        if (ms.Length < stream.Length)
                        {
                            fileInstance.IsCompressed = true;
                            saveStream = ms;
                        }
                    }

                    FileSystemRepository.SaveMetadata(fileInstance);
                    SaveFileToDb(saveStream, fileInstance.IdFile);
                }
            }
            catch (Exception e)
            {
                ReportImportStatus(string.Format("Произошла ошибка при сохранении файла в БД. {0}", e));
            }
        }

        private static void SaveFileToDb(Stream stream, int idFile)
        {
            if (!stream.CanSeek)
                throw new InvalidOperationException("Входной поток не поддерживает смещение позиции указателя чтения");

            stream.Seek(0, SeekOrigin.Begin);

            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    using (IDbCommand streamWriteCommand = connection.CreateCommand())
                    {
                        streamWriteCommand.CommandTimeout = 30000;
                        streamWriteCommand.CommandText =
                            String.Format("UPDATE {1} SET [{0}].WRITE(@data, @offset, @len) WHERE [idFileSystem]= @idFileSystem",
                                          FileDataName, FileTableName);

                        var dataParam = new SqlParameter("@data", SqlDbType.VarBinary);
                        streamWriteCommand.Parameters.Add(dataParam);

                        var offsetParam = new SqlParameter("@offset", SqlDbType.BigInt);
                        streamWriteCommand.Parameters.Add(offsetParam);

                        var lengthParam = new SqlParameter("@len", SqlDbType.BigInt);
                        streamWriteCommand.Parameters.Add(lengthParam);

                        var idParam = new SqlParameter("@idFileSystem", SqlDbType.Int);
                        streamWriteCommand.Parameters.Add(idParam);
                        idParam.Value = idFile;

                        var buffer = FileUtils.ExtGetOptimalBufferSize(stream.Length);
                        int read;
                        int offset = 0;

                        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            if (read == buffer.Length)
                            {
                                dataParam.Value = buffer;
                            }

                            else
                            {
                                var subBuffer = new byte[read];
                                Array.Copy(buffer, subBuffer, read);
                                dataParam.Value = subBuffer;
                            }

                            offsetParam.Value = offset;
                            lengthParam.Value = read;

                            streamWriteCommand.ExecuteNonQuery();
                            offset += read;
                        }
                    }

                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static string FileTableName { get; set; }

        public static string FileDataName { get; set; }

        private static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["TaxorgContext"].ConnectionString; }
        }

        private static string SaveCompressedFile(FileInfo fileInfo)
        {
            var tempFileName = Path.GetTempFileName();
            using (var sourceFileStream = File.OpenRead(fileInfo.FullName))
            {
                using (var destFileStream = File.Create(tempFileName))
                {
                    sourceFileStream.Compress(destFileStream);
                }
            }
            return tempFileName;
        }

        #endregion

        public static Stream GetStream(int idFile)
        {
            var fileInstance = new FSFileInstance(idFile);
            return GetStream(fileInstance);
        }

        private static Stream GetStream(FSFileInstance fileInstance)
        {
            var dataStream = GetStreamFromData(fileInstance.IdFile);
            Stream nonCompressedStream = null;

            if (fileInstance.IsCompressed)
            {
//                nonCompressedStream = DeCompressed(dataStream);

                nonCompressedStream = File.Create(Path.GetTempFileName());
//                nonCompressedStream = new MemoryStream();
                dataStream.DeCompress(nonCompressedStream);
            }

            return nonCompressedStream ?? dataStream;
        }

        private static Stream GetStreamFromData(int idFile)
        {
            var fileInstance = new FSFileInstance(idFile);
            return GetStreamFromData(fileInstance);
        }

        private static Stream GetStreamFromData(FSFileInstance fileInstance)
        {
            Stream myStream = new MemoryStream();
            string strSql = String.Format(@"SELECT {0} FROM {1} WHERE {2} = {3}",
                                          FSFile.FileDataField,
                                          BLMetadataContainer.GetMetadata(typeof (FSFile)).SourceData,
                                          FSFile.colIdFile, fileInstance.IdFile);

            var connection = new SqlConnection(Storage.DefaultStorage.ConnectionString);
            var command = new SqlCommand(strSql, connection) {CommandTimeout = 30000};
            connection.Open();
            var outByte = new byte[FileUtils.GZipDecompressBufferSize];

            SqlDataReader reader = command.ExecuteReader(CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                var writer = new BinaryWriter(myStream);
                long startIndex = 0;
                long retval = reader.GetBytes(0, startIndex, outByte, 0, FileUtils.GZipDecompressBufferSize);
                while (retval == FileUtils.GZipDecompressBufferSize)
                {
                    writer.Write(outByte);
                    writer.Flush();
                    startIndex += FileUtils.GZipDecompressBufferSize;
                    retval = reader.GetBytes(0, startIndex, outByte, 0, FileUtils.GZipDecompressBufferSize);
                }
                if (retval != 0)
                {
                    writer.Write(outByte, 0, (int) retval);
                }
                writer.Flush();
                myStream.Flush();
            }

            reader.Close();
            return myStream;
        }


        public static Stream DeCompressed(Stream dataStream)
        {
            Stream nonCompressedStream;

            var compressedTempFileName = Path.GetTempFileName();
            var nonCompressedTempFileName = Path.GetTempFileName();

            using (var fs = new FileStream(compressedTempFileName, FileMode.Create))
            {
                dataStream.CopyTo(fs);
            }

            FileUtils.Decompress(compressedTempFileName, nonCompressedTempFileName);

            using (var fs = new FileStream(nonCompressedTempFileName, FileMode.Open))
            {
                nonCompressedStream = new MemoryStream();
                fs.CopyTo(nonCompressedStream);
            }
            return nonCompressedStream;
        }

        public static void DownloadUnzipOpenFile(int idFile)
        {
            const string filePrefix = "~temp_";
            string fileFolder = FileUtils.ApplicationPath + "DownloadedFiles\\";

            if (!Directory.Exists(fileFolder))
            {
                Directory.CreateDirectory(fileFolder);
            }

            var fileInstance = new FSFileInstance(idFile);
            var fileName = fileInstance.FileName;
            string targetFile = FileUtils.GetFreeFileName(filePrefix, fileName, fileInstance.FileExtension, fileFolder);
            FileUtils.ClearTemporaryFiles(fileFolder, filePrefix, ExcelHelper.XlsFilesExtensions.ToArray());

            ReadFromDbToHdd(fileInstance, targetFile);

            FileUtils.OpenFile(targetFile, fileInstance.FileExtension);
        }

        internal static void ReadFromDbToHdd(int idFile, string targetFile)
        {
            var fileInstance = new FSFileInstance(idFile);
            ReadFromDbToHdd(fileInstance, targetFile);
        }

        internal static void ReadFromDbToHdd(FSFileInstance fileInstance, string targetFile)
        {
            if (targetFile == null) 
                throw new ArgumentNullException("targetFile");

            using (var fileStream = File.Create(targetFile))
            {
                var stream = GetStream(fileInstance);
                stream.CopyTo(fileStream);
            }
        }

        #endregion
    }
}