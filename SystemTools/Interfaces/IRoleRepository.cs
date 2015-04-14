namespace SystemTools.Interfaces
{
    public interface IRoleRepository : IQueryableCollection<IRole>
    {
        void Add(string roleName);
        void Edit(int idRole, string roleName);
        void Edit(string roleName, string newRoleName);
        void Delete(string roleName);
        void Delete(int idRole);
        IRole GetRole(int idRole);
        IRole GetRole(string roleName);
    }
}