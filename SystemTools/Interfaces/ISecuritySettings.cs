namespace SystemTools.Interfaces
{
    public interface ISecuritySettings
    {
        IdentificationMode IdentitficationMode { get; set; }
    }

    public enum IdentificationMode
    {
        None,
        WindowsOrForms,
        Windows,
        Forms
    }
}
