using ProtocolCryptographyD;
using System.Text;
using CryptL;
using ServerRepository;

namespace CommandsKit
{
    internal static class ExecuteRequest
    {
        public static bool Authentication(ref ClientInfo clientInfo, byte[] hashAuthentication)
        {
            RepositoryClient clientR = new RepositoryClient();
            Client? client = clientR.SelectForHash(hashAuthentication);

            bool answer = false;
            if (client != null)
            {
                answer = true;
                clientInfo.clientId = client.Id;
            }

            clientInfo.authentication = answer;           

            return answer;
        }
        public static bool Registration(string login, byte[] hashAuthentication)
        {
            RepositoryClient clientR = new RepositoryClient();
            Client? client = clientR.SelectForName(login);
            bool answer = false;

            if (client == null)
            {
                client = new Client(hashAuthentication, login);
                clientR.Add(client);
                clientR.SaveChange();

                answer = true;
            }

            return answer;
        }
        public static string Ls(ClientInfo clientInfo)
        {
            RepositoryClientFile clientFileR = new RepositoryClientFile();
            List<int> allFileId = clientFileR.IdFileForClient(clientInfo.clientId);
            StringBuilder lsInfo = new StringBuilder("");

            if(allFileId.Count > 0)
            {
                RepositoryFile fileR = new RepositoryFile();
                foreach(int id in allFileId)
                {
                    ServerRepository.File? file = fileR.SelectId(id);
                    if(file != null)
                    {
                        lsInfo.Append(file.Path);
                        lsInfo.Append('\n');
                    }
                }
            }

            return lsInfo.ToString();
        }
    }
}
