using RepositoryDB;

namespace ServerRepository
{
    public interface IRepositoryClientFile : IRepository<Client_File>
    {
        List<uint> IdFileForClient(uint id);
    }
}
