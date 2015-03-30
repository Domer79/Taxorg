namespace SystemTools.Interfaces
{
    public interface IUserRepository : IQueryableCollection<IUser>
    {
        void Add(string login, string domain, string password);
    }
}