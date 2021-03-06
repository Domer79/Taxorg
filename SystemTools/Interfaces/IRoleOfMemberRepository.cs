﻿namespace SystemTools.Interfaces
{
    public interface IRoleOfMemberRepository : IQueryableCollection<IRoleOfMember>
    {
        void AddMemberToRole(string memberName, string roleName);
        void AddMemberToRole(IMember member, IRole role);
        void AddMemberToRoleAsync(IMember member, IRole role);
        void DeleteMemberFromRole(string memberName, string roleName);
        void DeleteMemberFromRole(IMember member, IRole role);
        void DeleteMemberFromRoleAsync(IMember member, IRole role);
    }
}