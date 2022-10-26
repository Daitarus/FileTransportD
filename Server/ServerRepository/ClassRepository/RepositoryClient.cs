using RepositoryDB;

namespace ServerRepository
{
    public class RepositoryClient : Repository<Client>, IRepositoryClient
    {
        public RepositoryClient() : base (new ServerDB()) { }
        public Client? SelectForHash(byte[] hash)
        {
            ServerDB serverDB = (ServerDB)db;
            IQueryable<Client> clients = serverDB.Clients.Where(client => client.Hash.Equals(hash));
            if (clients.Count() > 0)
            {
                return clients.First();
            }
            else
            {
                return null;
            }
        }
    }
}
