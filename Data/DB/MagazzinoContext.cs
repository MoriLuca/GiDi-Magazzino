using DBUser;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DB
{
    public class MagazzinoContext : DbContext, DBUser.IMagazzinoContext
    {
        public DbSet<Prodotto> Prodotti { get; set; }
        public DbSet<Storico> Storici { get; set; }
        public DbSet<IProduttore> Produttori { get; set; }

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
                .HasIndex(i => new { i.CodiceArticolo, i.ProduttoreId} )
                .IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server= localhost;Database=MagazzinoGiDi;User Id=sa; Password=0000;");
        }

        public void AggiungiProdotto(int id, int numero)
        {
            using (var context = new DB.MagazzinoContext())
            {
                bool exist = context.Prodotti.Any(p => p.Id == id);
                if (exist)
                {
                    DB.Prodotto p = context.Prodotti.Single(i => i.Id == id);
                    for (int i = 0; i < numero; i++)
                    {
                        context.Storici.Add(new DB.Storico()
                        {
                            ProdottoId = p.Id,
                            DataInserimento = DateTime.Now,
                        });
                    }
                    int raws = context.SaveChanges();
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write($"Aggiornate {raws} righe sul database.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
                }
                else
                {

                }
            }
        }

        public void CreaNuovoProdotto(string codice, int produttoreId, decimal costoAquisto = 0, decimal prezzoVendita = 0)
        {
            using (var context = new DB.MagazzinoContext())
            {
                bool exist = context.Prodotti.Any(p => p.CodiceArticolo == codice);
                if (!exist)
                {
                    DB.Prodotto p = new DB.Prodotto()
                    {
                        ProduttoreId = produttoreId,
                        CodiceArticolo = codice.Trim().ToUpper()
                    };
                    context.Prodotti.Add(p);
                    context.SaveChanges();
                    Console.WriteLine("\nProdotto Registrato");
                    Console.Write("Inserire il numero pezzi da registrare : ");
                    string input = Console.ReadLine();
                    int numeroPezzi;
                    int id = context.Prodotti.Single(c => c.CodiceArticolo == codice.Trim().ToUpper()).Id;
                    if (int.TryParse(input, out numeroPezzi)) AggiungiProdotto(id, numeroPezzi);
                }
                else
                {
                    Console.Write("Il codice inserito è gia registrato");
                }
            }
        }

        public IProdotto RicercaProdotto(string codiceProdotto)
        {
            DB.Prodotto pr;
            using (var context = new DB.MagazzinoContext())
            {
                try
                {
                    pr = context.Prodotti.Include(i => i.Produttore)
                        .Single(n => n.CodiceArticolo == codiceProdotto);
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
            return pr;
        }

        public List<IProduttore> GetProduttori()
        {
            return Produttori.ToList();
        }
    }
}
