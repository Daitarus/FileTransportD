using CommandsKit;
using ProtocolCryptographyD;
using System.Net;
using ServerRepository;
using System.Configuration;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool flagStartServer = false;
            string connectionString = ConfigurationManager.AppSettings["connectionString"];
            if(connectionString != null)
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

                Console.WriteLine("Server running!");
                server.Start();
            }
            else
            {
                Console.WriteLine("Error: Server not running!");
            }
        }
    }
}