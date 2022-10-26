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

            if(pcdClient.Connect())
            {
                Console.WriteLine("Connect...");

                Command com = new AuthenticationComR(HashSHA256.GetHash(Encoding.UTF8.GetBytes("123")), pcdClient.clientInfo.hashSessionId);
                pcdClient.ExecuteAction(com);

                if(pcdClient.clientInfo.authentication)
                {
                    Console.WriteLine("Authentication...");


                }
                else
                {
                    Console.WriteLine("Not authentication!");
                }
            }
            else
            {
                Console.WriteLine("No connect!");
            }
            pcdClient.Disconnect();
        }
    }
}