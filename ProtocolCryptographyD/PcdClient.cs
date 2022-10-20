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

        public PcdClient(IPEndPoint serverEndPoint)
        {
            this.serverEndPoint = serverEndPoint;

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            aes = new CryptAES();
        }

        public void Connect(byte[] bytes)
        {
            try
            {
                socket.Connect(serverEndPoint);
                Transport transport = new Transport(socket);

                //get PublicKey RSA
                RsaPkeyCom rsaCom;
                if (RsaPkeyCom.ParseToCom(transport.GetData(), out rsaCom)) 
                {
                    CryptRSA rsa = new CryptRSA(rsaCom.publicKey, false);

                    //send aes key
                    AesKeyCom aesKeyCom = new AesKeyCom(aes.UnionKeyIV());
                    transport.SendData(rsa.Encrypt(aesKeyCom.ConvertToBytes()));
                }
            }
            catch(Exception e)
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
            catch { }
        }
    }
}
