using CommandsKit;
using CryptL;
using ProtocolTransport;
using ConsoleWorker;

namespace Client
{
    internal static class ClientAlgorithms
    {
        public static void AuthorizationAlg(PcdClient pcdClient)
        {
            CommandRequest comProtocol;
            string login, password;
            bool successfulAuthentication = false;

            while (!successfulAuthentication)
            {
                int comEnter = EnterData.EnterNumAction(new string[] { "Authentication", "Registration" });

                EnterData.EnterAuthorization(out login, out password);
                string authData = String.Format("{0}{1}", login, password);
                byte[] authHash = HashSHA256.GetHash(authData);

                switch (comEnter)
                {
                    case 1:
                        {
                            comProtocol = new AuthenticationComR(authHash, pcdClient.clientInfo.sessionId);
                            successfulAuthentication = pcdClient.ServeCommand(comProtocol);
                            if (!successfulAuthentication)
                            {
                                PrintMessage.PrintColorMessage("\nAuthentication isn't successful!\n\n", ConsoleColor.Red);
                            }
                            break;
                        }
                    case 2:
                        {
                            comProtocol = new RegistrationComR(login, authHash, pcdClient.clientInfo.sessionId);
                            if (pcdClient.ServeCommand(comProtocol))
                            {
                                PrintMessage.PrintColorMessage("\nRegistration is successful!\n\n", ConsoleColor.Green);
                            }
                            else
                            {
                                PrintMessage.PrintColorMessage("\nRegistration isn't successful!\n\n", ConsoleColor.Red);
                            }
                            break;
                        }
                }
            }
            PrintMessage.PrintColorMessage("\nAuthentication is successful!\n\n", ConsoleColor.Green);
        }

        public static void ActionAlg(PcdClient pcdClient)
        {
            bool noErrorConnection = true;
            while (noErrorConnection)
            {
                int comEnter = EnterData.EnterNumAction(new string[] { "Print my files", "Get file", "Add file" });
                CommandRequest com;

                switch (comEnter)
                {
                    case 1:
                        {
                            com = new LsComR(pcdClient.clientInfo.sessionId);
                            if (!pcdClient.ServeCommand(com))
                            {
                                noErrorConnection = false;
                                PrintMessage.PrintColorMessage("\nError connection!\n", ConsoleColor.Red);
                            }
                            break;
                        }
                    case 2:
                        {
                            int id = EnterData.EnterInt32Message("Enter file id: ", "Error: Incorrect data!\n");
                            com = new FileGetComR(id, pcdClient.clientInfo.sessionId);
                            if (!pcdClient.ServeCommands(com))
                            {
                                noErrorConnection = false;
                                PrintMessage.PrintColorMessage("\nError connection!\n", ConsoleColor.Red);
                            }
                            break;
                        }
                    case 3:
                        {
                            string path = "kali.7z";
                            
                            break;
                        }
                }
            }
        }
    }
}
