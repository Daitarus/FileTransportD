using RepositoryDB;
using Microsoft.EntityFrameworkCore;

namespace ServerRepository
{
    public class ServerDB : DB
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Client_File> FilesAttach { get; set; }
    }
}