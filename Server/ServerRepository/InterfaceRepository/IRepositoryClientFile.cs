using RepositoryDB;

namespace ServerRepository
{
    public interface IRepositoryClientFile : IRepository<Client_File>
    {
        List<int> IdFileForClient(int id);
    }
}
