using ProtocolCryptographyD;
using System.Net;
using CommandsKit;
using CryptL;
using System.Text;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            PcdClient pcdClient = new PcdClient(serverEndPoint, new ComParser());

            ICommand com = new AuthorizationComR(HashSHA256.GetHash(Encoding.UTF8.GetBytes("123")));
            if(pcdClient.Connect())
            {
                pcdClient.ExecuteCommand(com);
            }
            pcdClient.Disconnect();
        }
    }
}