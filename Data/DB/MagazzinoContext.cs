using DBUser;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DB
{
    public class MagazzinoContext : DbContext
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server= localhost;Database=MagazzinoGiDi;User Id=sa; Password=0000;");
            optionsBuilder.UseMySql("Server=localhost;Database=Magazzino;Uid=pi;Pwd=0000;");
        }

        public int AggiungiProdotto(int id, int numero)
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
                     return context.SaveChanges();
                }
                else
                {
                    return -1;
                }
            }
        }

        public int[] CreaNuovoProdotto(string codice, int produttoreId, int pezzi = 0, decimal costoAquisto = 0, decimal prezzoVendita = 0)
        {
            int[] ret = new int[2];
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
                    ret[0] = context.SaveChanges();
                    AggiungiProdotto(p.Id, pezzi);
                    ret[1] = context.SaveChanges();
                    return ret;
                }
                else
                {
                    return new int[] { -1, -1 };
                }
            }
        }

        public Prodotto RicercaProdotto(string codiceProdotto)
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
        public Prodotto RicercaProdotto(int id)
        {
            DB.Prodotto pr;
            using (var context = new DB.MagazzinoContext())
            {
                try
                {
                    pr = context.Prodotti.Include(i => i.Produttore)
                        .Single(n => n.Id == id);
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
            return pr;
        }

        public int RimuoviProdotto(int id, int numero)
        {
            using (var context = new DB.MagazzinoContext())
            {
                bool exist = context.Prodotti.Any(p => p.Id == id);
                if (exist)
                {
                    List<DB.Storico> s = context.Storici.Where(pr => pr.ProdottoId == id).Take(numero).ToList();
                    if (s != null) context.Storici.RemoveRange(s);
                    return context.SaveChanges();
                }
                else
                {
                    return -1;
                }

            }
        }

        public List<Produttore> GetProduttori()
        {
            return Produttori.ToList();
        }
    }
}
