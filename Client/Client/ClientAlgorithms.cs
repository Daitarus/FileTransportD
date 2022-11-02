using CommandsKit;
using CryptL;
using ProtocolCryptographyD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal static class ClientAlgorithms
    {
        public static void AuthorizationAlg(PcdClient pcdClient)
        {
            Command comProtocol;
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
            while (true)
            {
                int comEnter = EnterData.EnterNumAction(new string[] { "Print my files", "Get File" });
                Command com;

                switch (comEnter)
                {
                    case 1:
                        {
                            com = new LsComR(pcdClient.clientInfo.sessionId);
                            if(!pcdClient.ServeCommand(com))
                            {
                                PrintMessage.PrintColorMessage("Error connection!\n", ConsoleColor.Red);
                            }
                            break;
                        }
                    case 2:
                        {
                            int id = EnterData.EnterInt32Message("Enter file id: ", "Error: Incorrect data!\n");

                            com = new FileGetComR(id, pcdClient.clientInfo.sessionId);
                            bool noErrorConnection = pcdClient.SendCommand(com);
                            if (noErrorConnection)
                            {
                                
                                bool cycleGetFileBlock = true;

                                while (cycleGetFileBlock)
                                {
                                    com = pcdClient.GetCommand();

                                    noErrorConnection = (com != null);
                                    if (noErrorConnection)
                                    {
                                        noErrorConnection = (com.TypeCom == (byte)TypeCommand.FILE_GET_A);
                                        if (noErrorConnection)
                                        {
                                            FileGetComA comFile = (FileGetComA)com;
                                            noErrorConnection = pcdClient.ExecuteCommand(comFile);
                                            PrintMessage.PrintColorMessage(PrintMessage.GetLoadString("Download", comFile.numBlock, comFile.allBlock), ConsoleColor.White);
                                            if (noErrorConnection)
                                            {
                                                cycleGetFileBlock = !(comFile.numBlock == comFile.allBlock - 1);
                                            }
                                        }
                                    }
                                    if (!noErrorConnection)
                                    {
                                        cycleGetFileBlock = noErrorConnection;
                                    }
                                }
                                Console.WriteLine();
                            }   
                            if(!noErrorConnection)
                            {
                                PrintMessage.PrintColorMessage("Error conection!\n", ConsoleColor.Red);
                            }
                            break;
                        }
                }
            }
        }
    }
}
