using RepositoryDB;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerRepository
{
    [Table("Client_File")]
    public class Client_File : Entity
    {
        [Column("Id_Client")]
        [Required]
        public uint Id_Client { get; set; }

        [Column("Id_File")]
        [Required]
        public uint Id_File { get; set; }

        public Client_File(uint id_Client, uint id_File)
        {
            Id_Client = id_Client;
            Id_File = id_File;
        }
    }
}
