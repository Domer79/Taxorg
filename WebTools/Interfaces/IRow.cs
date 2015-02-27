using System;

namespace WebTools.Interfaces
{
    public interface IRow<out T> where T : class
    {
        T GetObject(CsvReaderErrorHandler errorHandler);
        T GetObject();
    }

}