using Microsoft.EntityFrameworkCore;
using System;

namespace DB
{
    public class MagazzinoContext : DbContext
    {
        public DbSet<Prodotto> Prodotti { get; set; }

        public MagazzinoContext(DbContextOptions<MagazzinoContext> options) : base(options)
        {
            //Database.EnsureDeleted();// Se esiste elimina il database
            Database.EnsureCreated();// Crea il database se non esiste

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Prodotto>()
                .HasIndex(i => i.CodiceArticolo)
                .IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server= localhost;Database=MagazzinoGiDi;User Id=sa; Password=0000;");
        }

    }
}
