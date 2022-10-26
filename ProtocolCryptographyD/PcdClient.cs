using CryptL;
using System.Net;
using System.Net.Sockets;

namespace ProtocolCryptographyD
{
    public class PcdClient
    {
        private IPEndPoint serverEndPoint;
        private Socket socket;
        private Transport transport;
        private IParser parser;

        public ClientInfo clientInfo = new ClientInfo();

        public PcdClient(IPEndPoint serverEndPoint, IParser parser)
        {
            this.serverEndPoint = serverEndPoint;

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);            
            this.parser = parser;
        }

        public bool Connect()
        {
            try
            {
                socket.Connect(serverEndPoint);

                clientInfo.timeConnection = DateTime.Now;
                clientInfo.endPoint = (IPEndPoint)socket.LocalEndPoint;
                clientInfo.aes = new CryptAES();

                transport = new Transport(socket);

                //get PublicKey RSA
                RsaPkeyCom rsaCom;
                if (RsaPkeyCom.ParseToCom(transport.GetData(), out rsaCom)) 
                {
                    CryptRSA rsa = new CryptRSA(rsaCom.publicKey, false);

                    //send aes key
                    AesKeyCom aesKeyCom = new AesKeyCom(clientInfo.aes.UnionKeyIV());
                    transport.SendData(rsa.Encrypt(aesKeyCom.ConvertToBytes()));

                    //get sessionId
                    SessionIdCom sessionIdCom;
                    if(SessionIdCom.ParseToCom(clientInfo.aes.Decrypt(transport.GetData()), out sessionIdCom))
                    {
                        clientInfo.hashSessionId = sessionIdCom.sessionId;

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

        public void ExecuteAction(Command com)
        {
            try
            {
                transport.SendData(clientInfo.aes.Encrypt(com.ToBytes()));
                com = parser.Parse(clientInfo.aes.Decrypt(transport.GetData()));
                com.ExecuteCommand(transport, ref clientInfo);
            }
            catch (Exception e)
            {
                Disconnect();
            }
        }

        public void Disconnect()
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception e)
            {
                
            }
        }
    }
}
