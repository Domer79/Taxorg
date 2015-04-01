using System.Linq;

namespace SystemTools.Interfaces
{
    public interface IUserRepository
    {
        void Add(string login, string password);
    }
}