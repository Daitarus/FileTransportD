using RepositoryDB;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerRepository
{
    [Table("Clients")]
    public class Client : Entity
    {
        [Column("Hash")]
        [Required]
        public byte[] Hash { get; set; }

        [Column("Name")]
        [MaxLength(255)]
        [Required]
        public string Name { get; set; }


        public Client(byte[] hash, string name)
        {
            Hash = hash;
            Name = name;
        }
    }
}
