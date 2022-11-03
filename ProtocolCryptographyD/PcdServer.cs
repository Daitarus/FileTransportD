﻿using CryptL;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ProtocolCryptographyD
{
    public class PcdServer
    {

        private IPEndPoint serverEndPoint;
        private CryptRSA rsa;
        private IParser parser;

        public PcdServer(IPEndPoint serverEndPoint, IParser parser)
        {
            this.serverEndPoint = serverEndPoint;
            rsa = new CryptRSA();
            this.parser = parser;
        }
        public PcdServer(IPEndPoint serverEndPoint, CryptRSA rsa, IParser parser)
        {
            this.serverEndPoint = serverEndPoint;
            this.rsa = rsa;
            this.parser = parser;
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
                    Socket client = listenSocket.Accept();
                    Task clientWork = new Task(() => ClientWork(client));
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
            //create client info
            ClientInfo clientInfo = CreateClientInfo((IPEndPoint)socket.RemoteEndPoint, DateTime.Now);

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
                    clientInfo.aes = new CryptAES(aesCom.unionKeyIV);

                    //send hash(SessionId)
                    SessionIdCom sessionIdCom = new SessionIdCom(clientInfo.sessionId);
                    transport.SendData(clientInfo.aes.Encrypt(sessionIdCom.ConvertToBytes()));

                    //client cycle
                    while(true)
                    {
                        Command com = parser.Parse(clientInfo.aes.Decrypt(transport.GetData()));
                        com.ExecuteCommand(ref transport, ref clientInfo);
                    }
                }

                Disconnect(socket);
            }
            catch (Exception e)
            {
                Disconnect(socket);
            }
        }

        private bool Disconnect(Socket socket)
        {
            try
            {
                socket.Disconnect(false);
                socket.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private ClientInfo CreateClientInfo(IPEndPoint clientEndPoint, DateTime dateConnection)
        {
            StringBuilder sessionIpStr = new StringBuilder();
            sessionIpStr.Append(clientEndPoint.ToString());
            sessionIpStr.Append(':');
            sessionIpStr.Append(dateConnection.Ticks);
            byte[] sessionId = Encoding.UTF8.GetBytes(sessionIpStr.ToString());

            ClientInfo clientInfo = new ClientInfo();
            clientInfo.endPoint = clientEndPoint;
            clientInfo.timeConnection = dateConnection;
            clientInfo.sessionId = HashSHA256.GetHash(sessionId);

            return clientInfo;
        }
    }
}
