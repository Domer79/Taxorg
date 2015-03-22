namespace SystemTools.Interfaces
{
    public interface IGrant
    {
        int IdSecObject { get; set; }

        int IdRole { get; set; }

        int IdAccessType { get; set; }
    }
}