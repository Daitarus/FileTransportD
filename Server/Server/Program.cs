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
            bool flagStartServer = false;
            string connectionString = ConfigurationManager.AppSettings["connectionString"];
            FileAddComR.directory = @"C:\Users\User\Desktop\DataServer\";

            if (connectionString != null)
            {
                ServerDB.ConnectionString = connectionString;
                if(ServerDB.CheckDB())
                {
                    flagStartServer = true;
                }
            }

            if (flagStartServer)
            {
                IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
                PcdServer server = new PcdServer(serverEndPoint, new ComParser());

                PrintMessage.PrintColorMessage("Server running!\n", ConsoleColor.Cyan);
                server.Start();
            }
            else
            {
                PrintMessage.PrintColorMessage("Error: Server not running!\n", ConsoleColor.Red);
            }
        }
    }
}