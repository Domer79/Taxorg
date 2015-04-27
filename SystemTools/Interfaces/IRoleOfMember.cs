namespace SystemTools.Interfaces
{
    public interface IRoleOfMember : IMember, IRole
    {
        bool IsUser { get; set; }
    }
}