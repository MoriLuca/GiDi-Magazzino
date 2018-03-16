using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DB
{
    public class SavingProduct : DB.Prodotto
    {
        public int NumeroPezzi { get; set; }
        public bool SalvaPezzo { get; set; }
    }

    public partial class MagazzinoContext 
    {
        #region Prodotto
        /// <summary>
        /// Aggiunta di un prodotto esistente
        /// </summary>
        /// <param name="id"></param>
        /// <param name="numero"></param>
        /// <returns></returns>
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
        public int AggiungiProdotto(string codice, int numero)
        {
            using (var context = new DB.MagazzinoContext())
            {
                bool exist = context.Prodotti.Any(p => p.CodiceArticolo == codice);
                if (exist)
                {

                    DB.Prodotto p = context.Prodotti.Single(i => i.CodiceArticolo == codice);
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
        /// <summary>
        /// Creazione di un nuovo prodotto
        /// </summary>
        /// <param name="codice"></param>
        /// <param name="produttoreId"></param>
        /// <param name="pezzi"></param>
        /// <param name="costoAquisto"></param>
        /// <param name="prezzoVendita"></param>
        /// <returns></returns>
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
        public int[] CreaNuovoProdotto(Prodotto p, int pezzi = 0)
        {
            int[] ret = new int[2];
            using (var context = new DB.MagazzinoContext())
            {
                bool exist = context.Prodotti.Any(pr => pr.CodiceArticolo == p.CodiceArticolo);
                if (!exist)
                {
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
        /// <summary>
        /// Ricerca di un prodotto esistente
        /// </summary>
        /// <param name="codiceProdotto"></param>
        /// <returns></returns>
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
                    pr = context.Prodotti.Include(i => i.Produttore).Include(i=>i.Storici)
                        .Single(n => n.Id == id);
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
            return pr;
        }
        /// <summary>
        /// Rimozione Prodotto
        /// </summary>
        /// <param name="id"></param>
        /// <param name="numero"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Restituisce tutti i prodotti salvati in memoria
        /// </summary>
        /// <returns></returns>
        public List<Prodotto> GetProdotti()
        {
            return Prodotti.Include(i=>i.Storici).ToList();
        }
        public List<Prodotto> GetProdotti(string codice)
        {
            return Prodotti.Include(i => i.Storici).Include(i=>i.Produttore).Where(i=>i.CodiceArticolo.Contains(codice)).ToList();
        }
        #endregion

        #region Produttore
        /// <summary>
        /// Restituisce l'elecnco di tutti i produttori
        /// </summary>
        /// <returns></returns>
        public List<Produttore> GetProduttori()
        {
            return Produttori.ToList();
        }
        public List<Produttore> GetProduttori(string nomeProduttore)
        {
            return Produttori.Where(p => p.Nome.Trim().Contains(nomeProduttore.ToUpper().Trim())).ToList();
        }
        public Produttore GetProduttore(int id)
        {
            return Produttori.Include(i => i.Prodotti).Single(n => n.Id == id);
        }
        public int AggiungiNuovoProduttore(Produttore pr )
        {
            using (var context = new DB.MagazzinoContext())
            {
                bool exist = context.Produttori.Any(p => p.Nome.Trim() == pr.Nome.ToUpper().Trim());
                if (!exist)
                {
                    context.Produttori.Add(pr);
                    return context.SaveChanges();
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write($"Il produttore ");
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" " + pr.Nome.ToUpper() + " ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write(" è gia presente in memoria.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
                    return -1;
                }
            }
        }
        #endregion
    }
}
