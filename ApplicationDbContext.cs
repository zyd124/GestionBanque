using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace GestionBanque
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Enregistrement> Enregistrements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("configuration.json")
                .Build();
            var connectionString = config.GetConnectionString("ApplicationConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
