using CryptL;
using System.Net;
using System.Net.Sockets;

namespace ProtocolCryptographyD
{
    public class PcdServer
    {
        private IPEndPoint serverEndPoint;
        private CryptRSA rsa;

        public PcdServer(IPEndPoint serverEndPoint)
        {
            this.serverEndPoint = serverEndPoint;
            rsa = new CryptRSA();
        }

        public void Start()
        {
            Socket listenSocket;

            try
            {
                while (true)
                {
                    listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    listenSocket.Bind(serverEndPoint);
                    listenSocket.Listen(1);
                    Socket clientSocket = listenSocket.Accept();
                    Task clientWork = new Task(() => ClientWork(clientSocket));
                    clientWork.Start();
                    listenSocket.Close();
                }
            }
            catch (Exception e)
            {

            }
        }

        private void ClientWork(Socket socket)
        {
            try
            {
                Transport transport = new Transport(socket);

                //send publicKey RSA
                RsaPkeyCom rsaPkeyCom = new RsaPkeyCom(rsa.PublicKey);
                transport.SendData(rsaPkeyCom.ConvertToBytes());

                //get AES key
                AesKeyCom aesCom;
                if (AesKeyCom.ParseToCom(rsa.Decrypt(transport.GetData()), out aesCom))
                {
                    CryptAES aes = new CryptAES(aesCom.unionKeyIV);

                    //cycle
                }
            }
            catch (Exception e)
            {
                Disconnect(socket);
            }
        }

        private void Disconnect(Socket socket)
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
