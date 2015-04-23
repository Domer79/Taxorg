namespace SystemTools.Interfaces
{
    public interface IRole
    {
        int IdRole { get; set; }

        string RoleName { get; set; }

        string Description { get; set; }
    }
}