using ProtocolTransport;
using System.Net;
using CommandsKit;
using ConsoleWorker;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            //IPEndPoint serverEndPoint = EnterData.EnterIpEndPoint();

            PcdClient pcdClient = new PcdClient(serverEndPoint, new ComParser());

            if(pcdClient.Connect())
            {
                PrintMessage.PrintColorMessage("Connection!\n\n", ConsoleColor.Cyan);
                ClientAlgorithms.AuthorizationAlg(pcdClient);
                ClientAlgorithms.ActionAlg(pcdClient);
            }
            else
            {
                PrintMessage.PrintColorMessage("Error: No connection!\n\n", ConsoleColor.Red);
            }
        }
    }
}