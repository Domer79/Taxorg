using System;

namespace SystemTools.Interfaces
{
    public interface IPublicRole : IRole
    {
        void GrantToRole(ISecObject secObject, Enum accessType);
        void GrantToRole(string secObject, string accessType);
    }
}