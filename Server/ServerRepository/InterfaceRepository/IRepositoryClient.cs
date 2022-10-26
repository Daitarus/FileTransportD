using RepositoryDB;

namespace ServerRepository
{
    public interface IRepositoryClient : IRepository<Client>
    {
        public Client? SelectForHash(byte[] hash);
    }
}
