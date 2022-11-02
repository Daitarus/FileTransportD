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
                        lsInfo.Append("Id:");
                        lsInfo.Append(file.Id);
                        lsInfo.Append(" Path:");
                        lsInfo.Append(file.Path);
                        lsInfo.Append('\n');
                    }
                }
            }

            return lsInfo.ToString();
        }
        public static bool FileGet(Transport transport, ClientInfo clientInfo, int fileId)
        {
            bool answer = false;

            RepositoryClientFile clientFileR = new RepositoryClientFile();
            List<int> filesId = clientFileR.IdFileForClient(clientInfo.clientId);

            foreach(int id in filesId)
            {
                if (id == fileId)
                {
                    answer = true;
                    break;
                }
            }

            if(answer)
            {
                RepositoryFile fileR = new RepositoryFile();
                ServerRepository.File file = fileR.SelectId(fileId);
                answer = (file != null);
                if (answer)
                {
                    FileInfo fileInfo = new FileInfo(file.FullPath);
                    byte[] fileInfoBytes = Encoding.UTF8.GetBytes(file.Name);
                    int MaxLengthBlock = FileGetComA.MaxLength_Info_Block - fileInfoBytes.Length;

                     answer = (fileInfo.Exists);
                    if (answer)
                    {
                        int numAllBlock = (int)Math.Ceiling((double)fileInfo.Length / (double)MaxLengthBlock);
                        answer = ((fileInfo.Length > 0) && (numAllBlock < 256));
                        if (answer)
                        {                            
                            using (FileStream fstream = System.IO.File.Open(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                for (byte i = 0; i < numAllBlock; i++)
                                {
                                    byte[] buffer = new byte[MaxLengthBlock];
                                    fstream.Seek(i * MaxLengthBlock, SeekOrigin.Begin);
                                    int numReadByte = fstream.Read(buffer);
                                    byte[] bufferFile = new byte[numReadByte];
                                    Array.Copy(buffer, 0, bufferFile, 0, numReadByte);


                                    Command com = new FileGetComA(i, (byte)numAllBlock, (byte)fileInfoBytes.Length, fileInfoBytes, bufferFile, clientInfo.sessionId);
                                    transport.SendData(clientInfo.aes.Encrypt(com.ToBytes()));
                                }
                            }
                        }
                    }
                }
            }

            if(!answer)
            {
                Command com = new FileGetComA(0, 1, 0, new byte[0], new byte[0], clientInfo.sessionId);
                transport.SendData(clientInfo.aes.Encrypt(com.ToBytes()));
            }

            return answer;
        }

    }
}
