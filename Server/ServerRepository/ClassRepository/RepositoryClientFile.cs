using RepositoryDB;

namespace ServerRepository
{
    public class RepositoryClientFile : Repository<Client_File>, IRepositoryClientFile
    {
        public RepositoryClientFile() : base(new ServerDB()) { }

        public List<int> IdFileForClient(int idClient)
        {
            ServerDB serverDB = (ServerDB)db;
;            IQueryable<Client_File> clientFileCollection = serverDB.FilesAttach.Where(clientFile => clientFile.Id_Client.Equals(idClient));

            List<int> allIdFile = new List<int>();

            if (clientFileCollection.Count() > 0)
            {
                foreach (var clientFile in clientFileCollection)
                {
                    allIdFile.Add(clientFile.Id_File);
                }                
            }

            return allIdFile;
        }
    }
}
