namespace SystemTools.Interfaces
{
    public interface IGrantDetail
    {
        int IdSecObject { get; set; }
        string ObjectName { get; set; }
        string ObjectDescription { get; set; }
        int IdRole { get; set; }
        string RoleName { get; set; }
        string RoleDescription { get; set; }
        int IdAccessType { get; set; }
        string AccessName { get; set; }
    }
}