using System;
using System.Data.Entity;
using System.Threading.Tasks;
using SystemTools.Interfaces;
using DataRepository;
using TaxorgRepository.Models;

namespace TaxorgRepository.Repositories
{
    public class SessionRepository: RepositoryBase<Session>
    {
        public override void InsertOrUpdate(Session item)
        {
            Set.Add(item);
        }

        protected override RepositoryDataContext GetContext()
        {
            return new TaxorgContext();
        }

        public static void AddNewSession(string sessionId, IUser user)
        {
            if (string.IsNullOrEmpty(sessionId)) 
                throw new ArgumentNullException("sessionId");

            if (user == null) 
                throw new ArgumentNullException("user");

            var repo = new SessionRepository();

            if (repo.Find(sessionId) != null)
                return;

            var session = new Session();
            session.SessionId = sessionId;
            session.UserId = user.IdUser;

            repo.InsertOrUpdate(session);
            repo.SaveChanges();
        }

        public static async void AddNewSessionAsync(string sessionId, IUser user)
        {
            await Task.Run(() => AddNewSession(sessionId, user));
        }
    }
}
