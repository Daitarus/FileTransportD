using RepositoryDB;

namespace ServerRepository
{
    public class RepositoryClientFile : Repository<Client_File>, IRepositoryClientFile
    {
        public RepositoryClientFile() : base(new ServerDB()) { }
        public List<Client_File> SelectClientId(uint idClient)
        {
            ServerDB serverDB = (ServerDB)db;
            IQueryable<Client_File> clientFileCollection = serverDB.FilesAttach.Where(clientFile => clientFile.Id_Client.Equals(idClient));

            return clientFileCollection.ToList();
        }

        public List<Client_File> SelectFileId(uint idFile)
        {
            ServerDB serverDB = (ServerDB)db;
            IQueryable<Client_File> clientFileCollection = serverDB.FilesAttach.Where(clientFile => clientFile.Id_File.Equals(idFile));

            return clientFileCollection.ToList();
        }

        public List<uint> IdFileForClient(uint idClient)
        {
            List<Client_File> clientFileCollection = SelectClientId(idClient);
            List<uint> allIdFile = new List<uint>();

            if (clientFileCollection.Count() > 0)
            {
                foreach (var clientFile in clientFileCollection)
                {
                    allIdFile.Add(clientFile.Id_File);
                }                
            }

            return allIdFile;
        }
        public List<uint> IdClientForFile(uint idFile)
        {
            List<Client_File> clientFileCollection = SelectFileId(idFile);
            List<uint> allIdClient = new List<uint>();

            if(clientFileCollection.Count() > 0)
            {
                foreach (var clientFile in clientFileCollection)
                {
                    allIdClient.Add(clientFile.Id_Client);
                }
            }

            return allIdClient;
        }
    }
}
