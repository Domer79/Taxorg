namespace SystemTools.Interfaces
{
    public interface IGroup : IMember
    {
        int IdGroup { get; set; }
        string GroupName { get; set; }
        string Description { get; set; }
    }
}