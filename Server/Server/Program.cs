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
            string ipString = ConfigurationManager.AppSettings["IpString"];
            string portString = ConfigurationManager.AppSettings["PortString"];

            IPAddress ip = new IPAddress(0);
            int port = 0;
      
            if (connectionString != null)
            {
                ServerDB.ConnectionString = connectionString;
                if(ServerDB.CheckDB())
                {
                    flagStartServer = IPAddress.TryParse(ipString, out ip) && int.TryParse(portString, out port);
                }
            }

            if (flagStartServer)
            {
                string fileHubPath = ConfigurationManager.AppSettings["FileHubPath"];
                if(fileHubPath != null)
                {
                    FileAddComR.Directory = fileHubPath;
                }
                IPEndPoint serverEndPoint = new IPEndPoint(ip, port);
                PcdServer server = new PcdServer(serverEndPoint, new ComParser());

                PrintMessage.PrintColorMessage(String.Format("Server running - {0}:{1}\n",ipString, portString), ConsoleColor.Cyan);
                server.Start();
            }
            else
            {
                PrintMessage.PrintColorMessage("Error: Server not running!\n", ConsoleColor.Red);
            }
        }
    }
}