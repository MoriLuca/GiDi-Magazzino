using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace UiConsole
{
    class Program
    {
        #region Properties
        static string gidi = "MagazzinoGiDi>";
        static int number;
        static string hr = new string('=', 100);
        static string ricercaUltimoProduttore ;

        static List<DB.Produttore> produttori = new List<DB.Produttore>();
        static List<DB.Prodotto> prodotti = new List<DB.Prodotto>();
        static DB.Produttore produttore = new DB.Produttore();
        #endregion

        static void Main(string[] args)
        {
            using (var context = new DB.MagazzinoContext())
            {

                SelezioneProduttore();
                string codiceArticolo = "rt4585";
                string nomeProduttore = "Delta";
                //Se trovo il prodotto, conosco gia il produttore, quindi posso creare lo storico
                DB.Prodotto prodotto = ResearchProdotto(codiceArticolo, nomeProduttore);
                DB.Produttore produttore = ResearchProduttore(nomeProduttore);

                //List<DB.Storico> storici = context.Storici.Include(i=>i.Prodotto).Include(i=>i.Prodotto.Produttore).ToList();


                if (prodotto != null)
                {
                    DB.Storico st = new DB.Storico()
                    {
                        DataInserimento = DateTime.Now,
                        Scopo = DB.Scopo.InternoGiDi,
                        Stato = DB.Stato.Nuovo,
                        ProdottoId = prodotto.Id
                    };
                    //creo il report storico
                    context.Storici.Add(st);
                    context.SaveChanges();
                }




                //Se non conosco il prodotto, ma conosco il produttore 
                else if (prodotto == null && produttore != null)
                {
                    //salvo il nuovo prodotto e poi procedo alla creazione dello storico
                    prodotto = new DB.Prodotto
                    {
                        Id = 0,
                        CodiceArticolo = codiceArticolo,
                        ProduttoreId = produttore.Id
                    };
                    context.Prodotti.Add(prodotto);

                    context.Storici.Add(new DB.Storico
                    {
                        Id = 0,
                        Prodotto = prodotto,
                        DataInserimento = DateTime.Now,
                        Scopo = DB.Scopo.Prestito,
                        Stato = DB.Stato.Rigenerato
                    });
                    context.SaveChanges();
                }
                // se non conosco ne produttore ne prodotto
                else
                {
                    produttore = new DB.Produttore
                    {
                        Id = 0,
                        Nome = nomeProduttore
                    };

                    //salvo il nuovo prodotto e poi procedo alla creazione dello storico
                    prodotto = new DB.Prodotto
                    {
                        Id = 0,
                        CodiceArticolo = codiceArticolo,
                        Produttore = produttore
                    };

                    context.Storici.Add(new DB.Storico
                    {
                        Id = 0,
                        DataInserimento = DateTime.Now,
                        Scopo = DB.Scopo.InternoGiDi,
                        Stato = DB.Stato.Nuovo,
                        Prodotto = prodotto
                    });
                    context.SaveChanges();
                }
            }
        }

        static void SelezioneProduttore()
        {
            #region display richiesta
            Console.Clear();
            using (var context = new DB.MagazzinoContext())
            {
                produttori = context.Produttori.OrderBy(p => p.Nome).ToList();
            }
            Console.WriteLine("Digitare man per il manuale.");
            Console.WriteLine(hr);
            //Display costruttori su console
            int perRaw = 4;
            int raw = 0;
            foreach (var item in produttori)
            {
                if(ricercaUltimoProduttore != null && item.Nome.ToUpper().Contains(ricercaUltimoProduttore))
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.Write(String.Format("{0,3}){1,-15}", item.Id, item.Nome));
                if (ricercaUltimoProduttore != null && item.Nome.ToUpper().Contains(ricercaUltimoProduttore))
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                raw++;
                if (raw == perRaw)
                {
                    Console.WriteLine();
                    raw = 0;
                }
            }
            if (raw < perRaw) Console.WriteLine();
            Console.WriteLine(hr);
            Console.Write(gidi);
            #endregion

            #region input
            string input = Console.ReadLine().Trim();

            if (int.TryParse(input, out number))
            {
                produttore = ResearchProduttore(number);
                if (produttore == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Inserito un numero produttore non valido.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
                    SelezioneProduttore();
                }
                else InsertNewProdotto();
            }
            else
            {

                //ricerca comando
                int comandLenght = input.IndexOf(" ");
                if (comandLenght == -1) input += " ";
                comandLenght = input.IndexOf(" ");
                string comando = input.Substring(0, comandLenght);
                string argument = input.Substring(input.IndexOf(" "));

                switch (comando)
                {
                    case "add":
                        InsertNewProduttore(argument);
                        break;
                    case "search":
                        SearchProduttore(argument);
                        break;
                    case "man":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Comandi validi :");
                        Console.WriteLine("add < nome_produttore > [ aggiungere un nuovo produttore ]");
                        Console.WriteLine("search < nome_produttore > [ ricerca produttori ]");
                        Console.WriteLine("exit [ chiusura applicazione ]");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ReadLine();
                        SelezioneProduttore();
                        break;
                    case "exit":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Comando non valido.");
                        Console.WriteLine("Comandi validi :");
                        Console.WriteLine("add < nome_produttore > [ aggiungere un nuovo produttore ]");
                        Console.WriteLine("search < nome_produttore > [ ricerca produttori ]");
                        Console.WriteLine("exit [ chiusura applicazione ]");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ReadLine();
                        SelezioneProduttore();
                        break;
                }

            }
            #endregion


        }

        public static void SearchProduttore(string nomeProduttore)
        {

            using (var context = new DB.MagazzinoContext())
            {
                int n  = context.Produttori.Where(p => p.Nome.Trim().Contains(nomeProduttore.ToUpper().Trim())).ToList().Count;
                if (n>0)
                {
                    ricercaUltimoProduttore = nomeProduttore.Trim().ToUpper();
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write($"Trovati ");
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" " + n + " ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write(" produttori che contengono il nome ");
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" " + nomeProduttore + " ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write(".");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
                }
                else
                {
                    ricercaUltimoProduttore = null;
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("Nessun produttore trovato con il nome che contiene ");
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" " + nomeProduttore.ToUpper() + " ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write(" .");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
                }
            }
            SelezioneProduttore();

        }

        public static void InsertNewProduttore(string nomeProduttore)
        {

            using (var context = new DB.MagazzinoContext())
            {
                bool exist = context.Produttori.Any(p => p.Nome.Trim() == nomeProduttore.ToUpper().Trim());
                if (!exist)
                {
                    context.Produttori.Add(new DB.Produttore
                    {
                        Nome = nomeProduttore.ToUpper().Trim()
                    });
                    context.SaveChanges();
                }
                else
                {
                    ricercaUltimoProduttore = nomeProduttore.Trim().ToUpper();
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write($"Il produttore ");
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" "+nomeProduttore.ToUpper()+" ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write(" è gia presente in memoria.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
                }
            }
            SelezioneProduttore();

        }

        static void InsertNewProdotto()
        {
            Console.Clear();
            Console.WriteLine($"Using Produttore {produttore.Nome}");
            using (var context = new DB.MagazzinoContext())
            {
                prodotti = context.Prodotti.OrderBy(p => p.CodiceArticolo).ToList();
            }
            Console.Clear();
            Console.WriteLine("Inserire il numero del codice registrato, oppure 'new' per crearne uno nuovo.");
            foreach (var item in prodotti)
            {
                Console.WriteLine($"({item.Id}){item.CodiceArticolo}");
            }
            Console.WriteLine();
            Console.Write(gidi);
            string input = Console.ReadLine().Trim();
            if (input == "new ") InsertNewProdotto();
            else if (Int32.TryParse(input, out number))
            {
                produttore = ResearchProduttore(number);
                if (produttore == null)
                {
                    Console.WriteLine("Inserito un numero produttore non valido.");
                    Console.ReadLine();
                    SelezioneProduttore();
                }

            }

        }

        static public DB.Produttore ResearchProduttore(string name)
        {
            DB.Produttore pr;
            using (var context = new DB.MagazzinoContext())
            {
                try
                {
                    pr = context.Produttori.Include(i => i.Prodotti).Single(n => n.Nome.ToLower() == name.ToLower());
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return pr;
        }
        static public DB.Produttore ResearchProduttore(int id)
        {
            DB.Produttore pr;
            using (var context = new DB.MagazzinoContext())
            {
                try
                {
                    pr = context.Produttori.Include(i => i.Prodotti).Single(n => n.Id == id);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return pr;
        }

        static public DB.Prodotto ResearchProdotto(string codiceArticolo, string nomeProduttore)
        {
            DB.Prodotto pr;
            using (var context = new DB.MagazzinoContext())
            {
                try
                {
                    pr = context.Prodotti.Include(i => i.Produttore)
                        .Single(n => n.CodiceArticolo == codiceArticolo && n.Produttore.Nome.ToLower() == nomeProduttore.ToLower());
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
            return pr;
        }
        static public DB.Prodotto ResearchProdotto(int id)
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
    }
}
