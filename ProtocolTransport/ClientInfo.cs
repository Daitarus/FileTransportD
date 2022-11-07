using System.Net;
using System.Text;
using CryptL;

namespace ProtocolTransport
{
    public class ClientInfo
    {
        public IPEndPoint endPoint;
        public DateTime timeConnection;
        public CryptAES aes;
        public byte[] sessionId;
        public bool authentication;
        public int clientId;

        public override string ToString()
        {
            return String.Format("{0}:{1} - {2}", endPoint.Address.ToString(), endPoint.Port.ToString(), Convert.ToBase64String(sessionId));
        }
    }
}
