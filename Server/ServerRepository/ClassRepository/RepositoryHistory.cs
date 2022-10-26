using RepositoryDB;

namespace ServerRepository
{
    public class RepositoryHistory : Repository<History>, IRepositoryHistory
    {
        public RepositoryHistory() : base(new ServerDB()) { }
    }
}
