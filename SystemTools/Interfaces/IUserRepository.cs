namespace SystemTools.Interfaces
{
    public interface IUserRepository : IQueryableCollection<IUser>
    {
        void Add(string login, string email, string displayName, string passwordOrSid);
        void Edit(string login, string email, string usersid, string displayName, string passwordOrSid);
        void Delete(string login, string password);
        void Delete(string login, string email, string password);
        void Delete(string login, string email, string usersid, string password);
    }
}