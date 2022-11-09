using CommandsKit;
using CryptL;
using ProtocolTransport;
using ConsoleWorker;
using System.Text;

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
                            comProtocol = new AuthenticationComR(authHash, pcdClient.sessionId);
                            successfulAuthentication = pcdClient.ServeCommand(comProtocol);
                            if (!successfulAuthentication)
                            {
                                PrintMessage.PrintColorMessage("\nAuthentication isn't successful!\n\n", ConsoleColor.Red);
                            }
                            break;
                        }
                    case 2:
                        {
                            comProtocol = new RegistrationComR(login, authHash, pcdClient.sessionId);
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

        public static void MainAlg(PcdClient pcdClient)
        {
            bool noErrorConnection = true;
            while (noErrorConnection)
            {
                int comEnter = EnterData.EnterNumAction(new string[] { "Print my files", "Get file", "Add file", "Delete file", "Set default directory for file" });
                CommandRequest com;

                switch (comEnter)
                {
                    case 1:
                        {
                            com = new LsComR(pcdClient.sessionId);
                            if (!pcdClient.ServeCommand(com))
                            {
                                noErrorConnection = false;
                                PrintMessage.PrintColorMessage("\nError connection!\n", ConsoleColor.Red);
                            }
                            break;
                        }
                    case 2:
                        {
                            uint id = EnterData.EnterUInt32Message("Enter file id: ", "Error: Incorrect data!\n");
                            com = new FileGetComR(id, pcdClient.sessionId);
                            if (!pcdClient.ServeCommands(com))
                            {
                                noErrorConnection = false;
                                PrintMessage.PrintColorMessage("\nError connection!\n", ConsoleColor.Red);
                            }
                            break;
                        }
                    case 3:
                        {
                            SendFile(EnterData.ChooseFile("Enter full name file: ", "Error: This file not exist!"), pcdClient);                            
                            break;
                        }
                    case 4:
                        {
                            uint id = EnterData.EnterUInt32Message("Enter file id: ", "Error: Incorrect data!\n");
                            com = new FileDeleteComR(id, pcdClient.sessionId);
                            if (!pcdClient.ServeCommand(com))
                            {
                                noErrorConnection = false;
                                PrintMessage.PrintColorMessage("\nError connection!\n", ConsoleColor.Red);
                            }
                            break;
                        }
                    case 5:
                        {
                            PrintMessage.PrintColorMessage("\nEnter path: ", ConsoleColor.White);
                            FileGetComA.Directory = Console.ReadLine();
                            break;
                        }
                }
            }
        }

        public static void SendFile(FileInfo fileInfo, PcdClient pcdClient)
        {
            string fileInfoStrServer = EnterData.EnterNotNullMessage("Enter file name to server: ", "Error: File name is empty!");
            byte[] fileInfoBytesServer = Encoding.UTF8.GetBytes(fileInfoStrServer);

            if (fileInfo.Length >= 0 && fileInfo.Length <= (Command.MaxLengthData * 255))
            {
                long MaxLengthBlock = FileGetComA.MaxLength_Info_Block - fileInfoBytesServer.Length;
                int numAllBlock = (int)Math.Ceiling((double)fileInfo.Length / (double)MaxLengthBlock);
                using (FileStream fstream = System.IO.File.Open(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    bool errorSend = false;
                    for (byte i = 0; i < numAllBlock; i++)
                    {
                        byte[] buffer = new byte[MaxLengthBlock];
                        long beginRead = i * MaxLengthBlock;
                        fstream.Seek(beginRead, SeekOrigin.Begin);
                        int numReadByte = fstream.Read(buffer);
                        byte[] bufferFile = new byte[numReadByte];
                        Array.Copy(buffer, 0, bufferFile, 0, numReadByte);

                        CommandRequest com = new FileAddComR(i, (byte)numAllBlock, (byte)fileInfoBytesServer.Length, fileInfoBytesServer, bufferFile, pcdClient.sessionId);
                        errorSend = !pcdClient.ServeCommand(com);

                        if(!errorSend)
                        {
                            PrintMessage.PrintColorMessage(CreatorOutString.GetLoadString(String.Format("Loading \"{0}\"", fileInfoStrServer), i, numAllBlock), ConsoleColor.White);
                        }
                        else
                        {
                            PrintMessage.PrintColorMessage("\nError: file not added!\n", ConsoleColor.Red);
                            break;
                        }
                    }
                    Console.Write("\n\n");
                }
            }
            else
            {
                PrintMessage.PrintColorMessage("\nError: file size is very small or very big!\n", ConsoleColor.Red);
            }
        }
    }
}
