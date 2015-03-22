using System.Linq;

namespace SystemTools.Interfaces
{
    public interface IQueryableCollection<out T> where T : class
    {
        IQueryable<T> GetQueryableCollection();
    }
}