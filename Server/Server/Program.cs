using CommandsKit;
using ProtocolTransport;
using System.Net;
using ServerRepository;
using System.Configuration;
using ConsoleWorker;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.AppSettings["connectionString"];
            string ipString = ConfigurationManager.AppSettings["IpString"];
            string portString = ConfigurationManager.AppSettings["PortString"];
            string createScriptPath = ConfigurationManager.AppSettings["createScriptPath"];
            string fileHubPath = ConfigurationManager.AppSettings["FileHubPath"];

            IPAddress ip; int port;

            if (CheckValidityStartData(connectionString, ipString, portString, createScriptPath, ref fileHubPath, out ip, out port))
            {               
                FileAddComR.Directory = fileHubPath;

                IPEndPoint serverEndPoint = new IPEndPoint(ip, port);
                PcdServer server = new PcdServer(serverEndPoint, new ComParser());

                PrintMessage.PrintColorMessage(String.Format("Server running - {0}:{1}\n",ipString, portString), ConsoleColor.Cyan);
                server.Start();
            }
            else
            {
                PrintMessage.PrintColorMessage("Error: Server not running!\n", ConsoleColor.Red);
                Console.ReadKey();
            }
        }

        static bool CheckValidityStartData(string connectionString, string ipString, string portString, string createScriptPath, ref string fileHubPath, out IPAddress ip, out int port)
        {
            ip = new IPAddress(0);
            port = 0;

            if (connectionString != null)
            {
                ServerDB.ConnectionString = connectionString;

                if (IPAddress.TryParse(ipString, out ip))
                {
                    if (int.TryParse(portString, out port))
                    {
                        bool flagDBConnection;
                        string createScript = null;

                        if (createScriptPath != null)
                        {
                            createScript = ServerDB.GetSqlScript(createScriptPath);
                        }
                        if (createScript == null)
                        {
                            flagDBConnection = ServerDB.CheckDB();
                        }
                        else
                        {
                            flagDBConnection = ServerDB.CheckDB(createScript);
                        }

                        if (flagDBConnection)
                        {
                            if(createScript == null)
                            {
                                PrintMessage.PrintColorMessage("Warning: Table creation script not loaded, if the database does not exist, it will be created by default!\n", ConsoleColor.Yellow);
                            }

                            if (fileHubPath == null)
                            {
                                fileHubPath = "";
                                PrintMessage.PrintColorMessage("Warning: File storage installed by default!\n", ConsoleColor.Yellow);
                            }

                            return true;
                        }
                        else
                        {
                            PrintMessage.PrintColorMessage("Error: No database connection!\n", ConsoleColor.Red);
                        }
                    }
                    else
                    {
                        PrintMessage.PrintColorMessage("Error: Invalid port information!\n", ConsoleColor.Red);
                    }
                }
                else
                {
                    PrintMessage.PrintColorMessage("Error: Invalid IP information!\n", ConsoleColor.Red);
                }
            }
            else
            {
                PrintMessage.PrintColorMessage("Error: Connection string not received!\n", ConsoleColor.Red);
            }

            return false;
        }
    }
}