using ProtocolCryptographyD;
using System.Net;
using CommandsKit;
using CryptL;
using System.Text;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            IPEndPoint serverEndPoint = EnterData.EnterIpEndPoint();

            PcdClient pcdClient = new PcdClient(serverEndPoint, new ComParser());

            if(pcdClient.Connect())
            {
                PrintMessage.PrintColorMessage("Connection!\n\n", ConsoleColor.Cyan);
                AuthorizationAlg(pcdClient);
            }
            else
            {
                PrintMessage.PrintColorMessage("Error: No connection!\n\n", ConsoleColor.Red);
            }
        }

        private static void AuthorizationAlg(PcdClient pcdClient)
        {
            int comEnter = EnterData.EnterNumAction(new string[] { "Authentication", "Registration" });
            Command comProtocol;
            bool successfulAuthentication = false;
            while (!successfulAuthentication)
            {
                string login, password;
                EnterData.EnterAuthorization(out login, out password);
                string authData = String.Format("{0}{1}", login, password);
                byte[] authHash = HashSHA256.GetHash(authData);

                switch (comEnter)
                {
                    case 1:
                        {                            
                            comProtocol = new AuthenticationComR(authHash, pcdClient.clientInfo.sessionId);
                            pcdClient.ExecuteAction(comProtocol);
                            successfulAuthentication = pcdClient.clientInfo.authentication;
                            if(!successfulAuthentication)
                            {
                                PrintMessage.PrintColorMessage("\nAuthentication isn't successful!\n\n", ConsoleColor.Red);
                            }
                            break;
                        }
                    case 2:
                        {
                            break;
                        }
                }
            }

            PrintMessage.PrintColorMessage("\nAuthentication is successful!\n\n", ConsoleColor.Green);
        }
    }
}