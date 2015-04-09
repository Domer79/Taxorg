using System.Linq;

namespace SystemTools.Interfaces
{
    public interface IGrantRepository : IQueryableCollection<IGrant>
    {
        void AddGrant(int idSecObject, int idRole, int idAccessType);
        void AddGrant(ISecObject securityObject, IRole role, IAccessType accessType);
        void RemoveGrant(int idSecObject, int idRole, int idAccessType);
        IQueryable<IRole> GetRoles();
        IQueryable<ISecObject> GetSecObjects();
        IQueryable<IAccessType> GetAccessTypes();
    }
}