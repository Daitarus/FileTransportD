using RepositoryDB;

namespace ServerRepository
{
    public interface IRepositoryFile : IRepository<File>
    {
        public File? GetToPath(string path);
    }
}
