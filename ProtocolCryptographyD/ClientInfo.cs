using System.Net;
using CryptL;

namespace ProtocolCryptographyD
{
    public class ClientInfo
    {
        public IPEndPoint endPoint;
        public DateTime timeConnection;
        public CryptAES aes;
        public byte[] sessionId;
        public byte[] hashSessionId;
        public bool authentication;
    }
}
