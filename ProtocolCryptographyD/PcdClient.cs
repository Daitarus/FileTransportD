using CryptL;
using System.Net;
using System.Net.Sockets;

namespace ProtocolCryptographyD
{
    public class PcdClient
    {
        private IPEndPoint serverEndPoint;
        private Socket socket;
        private CryptAES aes;
        private byte[] hashSessionId;
        private Transport transport;
        private IParser parser;

        public PcdClient(IPEndPoint serverEndPoint, IParser parser)
        {
            this.serverEndPoint = serverEndPoint;

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            aes = new CryptAES();
            this.parser = parser;
        }

        public bool Connect()
        {
            try
            {
                socket.Connect(serverEndPoint);
                transport = new Transport(socket);

                //get PublicKey RSA
                RsaPkeyCom rsaCom;
                if (RsaPkeyCom.ParseToCom(transport.GetData(), out rsaCom)) 
                {
                    CryptRSA rsa = new CryptRSA(rsaCom.publicKey, false);

                    //send aes key
                    AesKeyCom aesKeyCom = new AesKeyCom(aes.UnionKeyIV());
                    transport.SendData(rsa.Encrypt(aesKeyCom.ConvertToBytes()));

                    //get sessionId
                    SessionIdCom sessionIdCom;
                    if(SessionIdCom.ParseToCom(aes.Decrypt(transport.GetData()), out sessionIdCom))
                    {
                        hashSessionId = sessionIdCom.sessionId;

                        return true;
                    }
                }

                return false;
            }
            catch(Exception e)
            {
                Disconnect();
                return false;
            }
        }

        public void ExecuteCommand(ICommand com)
        {
            transport.SendData(aes.Encrypt(com.ToBytes()));
            com = parser.Parse(aes.Decrypt(transport.GetData()));
            com.ExecuteAction(transport, aes);
        }

        public void Disconnect()
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch { }
        }
    }
}
