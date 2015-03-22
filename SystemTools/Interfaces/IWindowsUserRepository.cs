namespace SystemTools.Interfaces
{
    public interface IUserRepository : IQueryableCollection<IUser>
    {
        void Add(string login, string email, string displayName, string usersid);
        void Edit(int idUser, string login, string email, string displayName, string usersid);
        void Edit(string login, string email, string displayName, string usersid);
        void Delete(string loginOrSid);
        void Delete(int idUser);
    }
}