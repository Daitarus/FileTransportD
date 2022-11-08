using System.Net;
using System.Text;

namespace ConsoleWorker
{
    public static class EnterData
    {
        public static IPEndPoint EnterIpEndPoint()
        {
            bool errorEnter = false;
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            int port = 5000;

            //enter ip
            while (!errorEnter)
            {
                PrintMessage.PrintColorMessage("Please, enter ip: ", ConsoleColor.White);
                errorEnter = IPAddress.TryParse(Console.ReadLine(), out ip);
                if (!errorEnter)
                {
                    PrintMessage.PrintColorMessage("Error: Incorrect data!\n", ConsoleColor.Red);
                }
            }
            errorEnter = false;

            //enter port
            while (!errorEnter)
            {
                PrintMessage.PrintColorMessage("Please, enter tcp port: ", ConsoleColor.White);
                errorEnter = int.TryParse(Console.ReadLine(), out port);
                if (!errorEnter)
                {
                    PrintMessage.PrintColorMessage("Error: Incorrect data!\n", ConsoleColor.Red);
                }
            }
            Console.WriteLine();

            return new IPEndPoint(ip, port);
        }

        public static string EnterNotNullMessage(string request, string answer)
        {
            bool errorEnter = true;
            string message = "";

            while (errorEnter)
            {
                PrintMessage.PrintColorMessage(request, ConsoleColor.White);
                message = Console.ReadLine();

                errorEnter = ((message == null) || (message == ""));
                if (errorEnter)
                {
                    PrintMessage.PrintColorMessage(String.Format("{0}\n", answer), ConsoleColor.Red);
                }
            }
            return message;
        }

        public static int EnterInt32Message(string request, string answer)
        {
            bool errorEnter = true;
            int message = 0;

            while (errorEnter)
            {
                PrintMessage.PrintColorMessage(request, ConsoleColor.White);
                errorEnter = !int.TryParse(Console.ReadLine(), out message);

                if (errorEnter)
                {
                    PrintMessage.PrintColorMessage(String.Format("{0}\n", answer), ConsoleColor.Red);
                }
            }

            return message;
        }
        public static uint EnterUInt32Message(string request, string answer)
        {
            bool errorEnter = true;
            uint message = 0;

            while (errorEnter)
            {
                PrintMessage.PrintColorMessage(request, ConsoleColor.White);
                errorEnter = !uint.TryParse(Console.ReadLine(), out message);

                if (errorEnter)
                {
                    PrintMessage.PrintColorMessage(String.Format("{0}\n", answer), ConsoleColor.Red);
                }
            }

            return message;
        }

        public static int EnterNumAction(string[] comStr)
        {
            bool errorEnter = true;
            int com = 0;

            while (errorEnter)
            {
                PrintMessage.PrintColorMessage("Choose comand:\n", ConsoleColor.Magenta);
                for (int i = 0; i < comStr.Length; i++)
                {
                    PrintMessage.PrintColorMessage(String.Format("{0}. {1}\n", i + 1, comStr[i]), ConsoleColor.Yellow);
                }
                errorEnter = !int.TryParse(Console.ReadLine(), out com);
                if (!errorEnter)
                {
                    if (!((com >= 1) && (com <= comStr.Length)))
                    {
                        errorEnter = true;
                    }
                }
                if (errorEnter)
                {
                    PrintMessage.PrintColorMessage("Error: Invalid command!\n", ConsoleColor.Red);
                }
            }

            return com;
        }

        public static string EnterSecureString()
        {
            StringBuilder message = new StringBuilder();
            ConsoleKeyInfo keyInfo;

            while (true)
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (message.Length != 0)
                    {
                        message.Remove(message.Length - 1, 1);
                    }
                }
                else if (keyInfo.Key >= ConsoleKey.D0 && keyInfo.Key <= ConsoleKey.Z)
                {
                    message.Append(keyInfo.KeyChar);
                }
            }

            return message.ToString();
        }

        public static string EnterNotNullMessageSecure(string request, string answer)
        {
            bool errorEnter = true;
            string message = "";

            while (errorEnter)
            {
                PrintMessage.PrintColorMessage(request, ConsoleColor.White);
                message = EnterSecureString(); ;

                errorEnter = (message == null || message.Length == 0);
                if (errorEnter)
                {
                    PrintMessage.PrintColorMessage(String.Format("{0}\n", answer), ConsoleColor.Red);
                }
            }
            return message;
        }

        public static void EnterAuthorization(out string login, out string password)
        {
            login = EnterNotNullMessage("Please, enter your login: ", "Error: Login is empty!");
            password = EnterNotNullMessageSecure("Please, enter your password: ", "Error: Password is empty!");
        }

        public static FileInfo ChooseFile(string request, string answer)
        {
            bool errorEnter = true;
            FileInfo fileInfo = new FileInfo("default");

            while (errorEnter)
            {
                string fileInfoStr = EnterNotNullMessage(request, "Error: File name is empty ");
                fileInfo = new FileInfo(fileInfoStr);
                errorEnter = (!fileInfo.Exists);
                if (errorEnter)
                {
                    PrintMessage.PrintColorMessage(String.Format("{0}\n", answer), ConsoleColor.Red);
                }
            }
            return fileInfo;
        }
    }
}
