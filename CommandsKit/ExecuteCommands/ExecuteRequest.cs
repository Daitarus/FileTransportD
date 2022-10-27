using ProtocolCryptographyD;
using System.Text;
using CryptL;
using ServerRepository;

namespace CommandsKit
{
    internal static class ExecuteRequest
    {
        public static bool Authentication(Transport transport, ref ClientInfo clientInfo, byte[] sessionId, byte[] hashAuthentication)
        {
            RepositoryClient clientR = new RepositoryClient();
            Client? client = clientR.SelectForHash(hashAuthentication);
            bool answer = (client != null);

            AuthenticationComA com = new AuthenticationComA(answer, sessionId);
            clientInfo.authentication = answer;

            transport.SendData(clientInfo.aes.Encrypt(com.ToBytes()));

            return answer;
        }
        public static bool Registration(Transport transport, ref ClientInfo clientInfo, byte[] sessionId, string login, byte[] hashAuthentication)
        {
            RepositoryClient clientR = new RepositoryClient();
            Client? client = clientR.SelectForName(login);
            RegistrationComA com;
            bool answer = false;

            if (client == null)
            {
                client = new Client(hashAuthentication, login);
                clientR.Add(client);
                clientR.SaveChange();

                answer = true;
            }

            com = new RegistrationComA(answer, sessionId);
            transport.SendData(clientInfo.aes.Encrypt(com.ToBytes()));

            return answer;
        }
    }
}
