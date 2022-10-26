using RepositoryDB;

namespace ServerRepository
{
    public class RepositoryFile : Repository<File>, IRepositoryFile
    {
        public RepositoryFile() : base(new ServerDB()) { }
        public File? GetToPath(string path)
        {
            ServerDB serverDB = (ServerDB)db;
            IQueryable<File> fileC = serverDB.Files.Where(file => file.Path.Equals(path));
            if (fileC.Count() > 0)
            {
                return fileC.First();
            }
            else
            {
                return null;
            }
        }
    }
}
