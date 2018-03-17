using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DB
{
    public partial  class MagazzinoContext : DbContext
    {
        public DbSet<Prodotto> Prodotti { get; set; }
        public DbSet<Storico> Storici { get; set; }
        public DbSet<Produttore> Produttori { get; set; }

        public MagazzinoContext(DbContextOptions<MagazzinoContext> options) : base(options)
        {
            //Database.EnsureDeleted();// Se esiste elimina il database
            Database.EnsureCreated();// Crea il database se non esiste

        }
        public MagazzinoContext()
        {
            //Database.EnsureDeleted();// Se esiste elimina il database
            Database.EnsureCreated();// Crea il database se non esiste

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Prodotto>()
                .HasIndex(i => new { i.CodiceArticolo, i.ProduttoreId })
                .IsUnique();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server= localhost;Database=MagazzinoGiDi;User Id=sa; Password=0000;");
        //    //optionsBuilder.UseMySql("Server=localhost;Database=Magazzino;Uid=pi;Pwd=0000;");
        //    //optionsBuilder.UseMySql("Server=192.168.1.7;Database=Magazzino;Uid=pi;Pwd=0000;");
        //}

    }
}
