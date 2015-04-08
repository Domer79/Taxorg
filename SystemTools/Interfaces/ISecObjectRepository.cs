namespace SystemTools.Interfaces
{
    public interface ISecObjectRepository<out TSecObject> : IQueryableCollection<TSecObject> 
        where TSecObject : class, ISecObject
    {

    }
}