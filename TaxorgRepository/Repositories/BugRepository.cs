using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataRepository;
using TaxorgRepository.Models;

namespace TaxorgRepository.Repositories
{
    public class BugRepository : RepositoryBase<Bug>
    {
        private static BugRepository _buges;

        public void SaveBugRow(string bugRow, string cause)
        {
            var bug = new Bug{ErrorData = bugRow, Cause = cause};
            InsertOrUpdate(bug);
            SaveChanges();
        }

        public void TakeOff(int idBug)
        {
            var bug = GetObjectByKey(idBug);
            bug.Accept = true;
            SaveChanges();
        }

        public BugRepository()
        {
        }

        public static BugRepository Buges
        {
            get { return _buges ?? (_buges = new BugRepository()); }
        }

        public override Expression Expression
        {
            get { return Set.Where(e => !e.Accept).Expression; }
        }

        protected override DbContext GetContext()
        {
            return new TaxorgContext();
        }
    }

    public class Buges
    {
        public static void SaveBugRow(string bugRow, string cause)
        {
            BugRepository.Buges.SaveBugRow(bugRow, cause);
        }

        public static void TakeOff(int idBug)
        {
            BugRepository.Buges.TakeOff(idBug);
        }

        private Buges()
        {
        }
    }
}
