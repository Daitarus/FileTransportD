using ProtocolCryptographyD;
using System.Text;
using CryptL;
using ServerRepository;

namespace CommandsKit
{
    internal static class ExecuteRequest
    {
        public static void Authentication(Transport transport, ref ClientInfo clientInfo, byte[] sessionId, byte[] hashAuthentication)
        {
            RepositoryClient clientR = new RepositoryClient();
            Client? client = clientR.SelectForHash(hashAuthentication);
            bool answer = (client != null);

            AuthenticationComA com = new AuthenticationComA(answer, sessionId);
            clientInfo.authentication = answer;

            transport.SendData(clientInfo.aes.Encrypt(com.ToBytes()));
        }
    }
}
