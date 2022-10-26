using RepositoryDB;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerRepository
{
    [Table("Histories")]
    public class History : Entity
    {
        [Column("Address")]
        [MaxLength(22)]
        [Required]
        public string Address { get; set; }

        [Column("Time")]
        [Required]
        public DateTime Time { get; set; }

        [Column("Action")]
        [Required]
        public string Action { get; set; }

        [Column("Id_Client")]
        [Required]
        public int Id_Client { get; set; }



        public History(string address, DateTime time, string action, int id_Client)
        {
            Address = address;
            Time = time.ToUniversalTime();
            Action = action;
            Id_Client = id_Client;
        }
    }
}
