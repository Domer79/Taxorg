namespace SystemTools.Interfaces
{
    public interface IRoleRepository : IQueryableCollection<IRole>
    {
        void Add(string roleName, string description);
        void Edit(int idRole, string roleName, string description);
        void Edit(string roleName, string newRoleName, string newDescription);
        void Delete(string roleName);
        void Delete(int idRole);
        IRole GetRole(int idRole);
        IRole GetRole(string roleName);
    }
}