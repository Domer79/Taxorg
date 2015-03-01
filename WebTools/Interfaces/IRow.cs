using System;

namespace SystemTools.Interfaces
{
    public interface IRow<out T> where T : class
    {
        T GetObject(CsvReaderErrorHandler errorHandler);
        T GetObject();
    }

}