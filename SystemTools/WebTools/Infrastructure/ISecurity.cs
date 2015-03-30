using SystemTools.Interfaces;

namespace SystemTools.WebTools.Infrastructure
{
    public interface ISecurity
    {
        IUser Identification(string login, string password);
    }
}