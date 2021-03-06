﻿using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Web;
using DataRepository.Infrastructure;
using TaxorgRepository.Models;
using SystemTools;

namespace TaxorgRepository.Repositories
{
    public static class FileSystemRepository
    {
        public static IQueryable<FileSystem> GetObjects()
        {
            var context = new TaxorgContext();
            return context.FileSystems;
        }

        public static void InsertOrUpdate(FileSystem item)
        {
            var context = new TaxorgContext();
            var fs = context.FileSystems.FirstOrDefault(f => f.IdFileSystem == item.IdFileSystem);
            if (fs != null)
            {
                context.Entry(item).State = EntityState.Modified;
                return;
            }

            fs = new FileSystem();
            fs.FileName = item.FileName;
            fs.RemoteHostFileName = item.RemoteHostFileName;
            fs.IsCompressed = item.IsCompressed;

            context.FileSystems.Add(fs);
        }

        public static int FileSave(HttpPostedFileBase file, string fullPath)
        {
            var fs = new FileSystem();
            fs.FileName = fullPath;
            fs.ContentType = file.ContentType;

//            SystemLogs.SaveLog(string.Format("FileName: [{0}]", fs.FileName));
            return FileSave(file.InputStream, fs);
        }

        public static int FileSave(Stream stream, FileSystem fileSystem)
        {
            SaveMetadata(fileSystem);
            SaveFileToDb(stream, fileSystem.IdFileSystem);
            return fileSystem.IdFileSystem;
        }

        public static void Delete(FileSystem item)
        {
            throw new NotImplementedException();
        }

        public static void SaveMetadata(FileSystem fileInstance)
        {
            var context = new TaxorgContext();
            var fileSystem = context.FileSystems.SingleOrDefault(fs => fs.IdFileSystem == fileInstance.IdFileSystem);

            if (fileSystem == null)
            {
                context.FileSystems.Add(fileInstance);
                context.SaveChanges();
            }
            else
            {
                context.Entry(fileInstance).State = EntityState.Modified;

            }
            var fsFile = context.FsFiles.SingleOrDefault(f => f.IdFileSystem == fileInstance.IdFileSystem);
            if (fsFile == null)
            {
                context.FsFiles.Add(new FsFile {IdFileSystem = fileInstance.IdFileSystem});
            }
            else
            {
                context.Entry(fsFile).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        private static void PrepareRow(int idFileSystem)
        {
            //1. Вставка пустого значения (чтобы в дальнейшем писать по буфферизации)
            string sqlCommandText = String.Format("UPDATE {2} SET {0} = 0x0 WHERE idFileSystem = {1}",
                FileDataName, idFileSystem, FileTableName);

            using (var connection = new SqlConnection(ApplicationSettings.ConnectionString))
            {
                connection.Open();
                var insertEmptyVarbinaryDataCommand = new SqlCommand(sqlCommandText, connection);
                insertEmptyVarbinaryDataCommand.ExecuteNonQuery();
            }
        }

        private static void SaveFileToDb(Stream stream, int idFileSystem)
        {
            if (!stream.CanSeek)
                throw new InvalidOperationException("Входной поток не поддерживает смещение позиции указателя чтения");

            stream.Seek(0, SeekOrigin.Begin);

            PrepareRow(idFileSystem);

            using (var connection = new SqlConnection(ApplicationSettings.ConnectionString))
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
                        idParam.Value = idFileSystem;

                        var buffer = ExtGetOptimalBufferSize(stream.Length);
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

        #region Read

        public const int GZipDecompressBufferSize = 8232960;
        public static Stream GetStreamFromData(int idFileSystem)
        {
            Stream myStream = new MemoryStream();
            string strSql = String.Format(@"SELECT {0} FROM {1} WHERE idFileSystem = {2}",
                                          FileDataName, FileTableName, idFileSystem);

            var connection = new SqlConnection(ApplicationSettings.ConnectionString);
            var command = new SqlCommand(strSql, connection) {CommandTimeout = 30000};
            connection.Open();
            var outByte = new byte[GZipDecompressBufferSize];

            SqlDataReader reader = command.ExecuteReader(CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                var writer = new BinaryWriter(myStream);
                long startIndex = 0;
                long retval = reader.GetBytes(0, startIndex, outByte, 0, GZipDecompressBufferSize);
                while (retval == GZipDecompressBufferSize)
                {
                    writer.Write(outByte);
                    writer.Flush();
                    startIndex += GZipDecompressBufferSize;
                    retval = reader.GetBytes(0, startIndex, outByte, 0, GZipDecompressBufferSize);
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
        
        #endregion

        public static string FileTableName = "FsFile";

        public static string FileDataName = "data";

        public static byte[] ExtGetOptimalBufferSize(long fileLength)
        {
            const int chunkBufferSize = 8040;
            byte[] buffer;

            if (fileLength >= chunkBufferSize * 1024)
            {
                buffer = new byte[chunkBufferSize * 1024];
            }

            else if (fileLength >= chunkBufferSize * 100)
            {
                buffer = new byte[chunkBufferSize * 100];
            }

            else if (fileLength >= chunkBufferSize * 25)
            {
                buffer = new byte[chunkBufferSize * 25];
            }

            else
            {
                buffer = new byte[fileLength];
            }
            return buffer;
        }
    }
}
