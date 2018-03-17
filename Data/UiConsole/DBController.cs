using System;
using System.Collections.Generic;
using System.Text;

namespace UiConsole
{
    public class DBController
    {
        private readonly DB.IMagazzino _magazzino;

        public DBController(DB.IMagazzino _magazzino)
        {
            _magazzino = _magazzino;
        }

        /// <summary>
        /// Inserire un nuovo produttore
        /// </summary>
        /// <param name="nomeProduttore"></param>
        public void InsertNewProduttore(string nomeProduttore)
        {
            _magazzino.AggiungiNuovoProduttore(new DB.Produttore()
            {
                Nome = nomeProduttore.Trim().ToUpper(),
            });
        }
        /// <summary>
        /// Restituisce un produttore in base all'id fornito
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DB.Produttore GetProduttore(int id)
        {
            return _magazzino.GetProduttore(id);
        }



        public void NewProdotto(string codice, int pezzi = 0)
        {
            int[] raws = _magazzino.CreaNuovoProdotto(codice, 1, pezzi);
            Console.WriteLine(raws[0].ToString(), raws[1]);
        }
        public void AddProdotto(string id, int numero)
        {
            Int16 i;
            int raws;
            if (Int16.TryParse(id, out i))
            {
                raws = _magazzino.AggiungiProdotto(i, numero);
            }
            else
            {
                raws = _magazzino.AggiungiProdotto(id, numero);
            }

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($"Aggiornate {raws} righe sul database.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
        }
        public void RemoveProdotto(int id, int numero)
        {
            int raws = _magazzino.RimuoviProdotto(id, numero);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($"Aggiornate {raws} righe sul database.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
        }
        public DB.Prodotto ResearchProdotto(string codiceArticolo)
        {
            return _magazzino.RicercaProdotto(codiceArticolo);
        }
        public DB.Prodotto ResearchProdotto(int id)
        {
            return _magazzino.RicercaProdotto(id);
        }

    }
}
