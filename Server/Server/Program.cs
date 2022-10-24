using CommandsKit;
using ProtocolCryptographyD;
using System.Net;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            PcdServer server = new PcdServer(serverEndPoint, new ComParser());
            server.Start();
        }
    }
}