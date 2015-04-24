namespace SystemTools.Interfaces
{
    public interface IUserGroupsDetailRepository : IQueryableCollection<IUserGroupsDetail>
    {
        void AddToGroup(string login, string groupName);
        void AddToGroup(int idUser, int idGroup);
        void AddToGroup(IUser user, IGroup group);
        void DeleteFromGroup(string userName, string groupName);
        void DeleteFromGroup(int idUser, int idGroup);
        void DeleteFromGroup(IUser user, IGroup group);
    }
}