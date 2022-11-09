using RepositoryDB;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace ServerRepository
{
    [Table("Histories")]
    public class History : Entity
    {
        [Column("Ip")]
        [Required]
        public IPAddress Ip { get; set; }

        [Column("Port")]
        [Required]
        public int Port { get; set; }

        [Column("Time")]
        [Required]
        public DateTime Time { get; set; }

        [Column("Action")]
        [Required]
        public string Action { get; set; }

        [Column("Id_Client")]
        [Required]
        public uint Id_Client { get; set; }


        public History(IPAddress ip, int port, DateTime time, string action, uint id_Client)
        {
            Ip = ip;
            Port = port;
            Time = time.ToUniversalTime();
            Action = action;
            Id_Client = id_Client;
        }
        public History(IPEndPoint endPoint, DateTime time, string action, uint id_Client)
        {
            Ip = endPoint.Address;
            Port = endPoint.Port;
            Time = time.ToUniversalTime();
            Action = action;
            Id_Client = id_Client;
        }
    }
}
