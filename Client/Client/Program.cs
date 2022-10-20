using ProtocolCryptographyD;
using System.Net;
using System.Text;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            PcdClient pcdClient = new PcdClient(serverEndPoint);
            pcdClient.Connect(Encoding.UTF8.GetBytes("ggg127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1127.0.0.1"));
            pcdClient.Disconnect();
        }
    }
}