using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace UiConsole
{
    class Program
    {
        static DB.MagazzinoContext Magazzino = new DB.MagazzinoContext();

        #region Properties
        static string gidi = "MagazzinoGiDi>";
        static int number;
        static string hr = new string('=', 100);
        static string ricercaUltimoProduttore;
        static string ricercaUltimoProdotto;

        static List<DBUser.IProduttore> produttori = new List<DBUser.IProduttore>();
        static List<DB.Prodotto> prodotti = new List<DB.Prodotto>();
        static DBUser.IProduttore produttore = new DB.Produttore();
        #endregion

        static void Main(string[] args)
        {
                SelezioneProduttore();
                string codiceArticolo = "rt4585";
                string nomeProduttore = "Delta";
                //Se trovo il prodotto, conosco gia il produttore, quindi posso creare lo storico
                DBUser.IProdotto prodotto = ResearchProdotto(codiceArticolo);
                DBUser.IProduttore produttore = ResearchProduttore(nomeProduttore);

                //List<DB.Storico> storici = context.Storici.Include(i=>i.Prodotto).Include(i=>i.Prodotto.Produttore).ToList();


                //if (prodotto != null)
                //{
                //    DB.Storico st = new DB.Storico()
                //    {
                //        DataInserimento = DateTime.Now,
                //        Scopo = DB.Scopo.InternoGiDi,
                //        Stato = DB.Stato.Nuovo,
                //        ProdottoId = prodotto.Id
                //    };
                //    //creo il report storico
                //    context.Storici.Add(st);
                //    context.SaveChanges();
                //}




                ////Se non conosco il prodotto, ma conosco il produttore 
                //else if (prodotto == null && produttore != null)
                //{
                //    //salvo il nuovo prodotto e poi procedo alla creazione dello storico
                //    prodotto = new DB.Prodotto
                //    {
                //        Id = 0,
                //        CodiceArticolo = codiceArticolo,
                //        ProduttoreId = produttore.Id
                //    };
                //    context.Prodotti.Add(prodotto);

                //    context.Storici.Add(new DB.Storico
                //    {
                //        Id = 0,
                //        Prodotto = prodotto,
                //        DataInserimento = DateTime.Now,
                //        Scopo = DB.Scopo.Prestito,
                //        Stato = DB.Stato.Rigenerato
                //    });
                //    context.SaveChanges();
                //}
                //// se non conosco ne produttore ne prodotto
                //else
                //{
                //    produttore = new DB.Produttore
                //    {
                //        Id = 0,
                //        Nome = nomeProduttore
                //    };

                //    //salvo il nuovo prodotto e poi procedo alla creazione dello storico
                //    prodotto = new DB.Prodotto
                //    {
                //        Id = 0,
                //        CodiceArticolo = codiceArticolo,
                //        Produttore = produttore
                //    };

                //    context.Storici.Add(new DB.Storico
                //    {
                //        Id = 0,
                //        DataInserimento = DateTime.Now,
                //        Scopo = DB.Scopo.InternoGiDi,
                //        Stato = DB.Stato.Nuovo,
                //        Prodotto = prodotto
                //    });
                //    context.SaveChanges();
                //}
            }
        

        #region produttore
        static void SelezioneProduttore()
        {
            #region display richiesta
            Console.Clear();
            produttori = Magazzino.GetProduttori();
            
            Console.WriteLine("Digitare man per il manuale.");
            Console.WriteLine(hr);
            //Display costruttori su console
            int perRaw = 4;
            int raw = 0;
            foreach (var item in produttori)
            {
                if (ricercaUltimoProduttore != null && item.Nome.ToUpper().Contains(ricercaUltimoProduttore))
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
                else MenuProdotto();
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
                int n = context.Produttori.Where(p => p.Nome.Trim().Contains(nomeProduttore.ToUpper().Trim())).ToList().Count;
                if (n > 0)
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
                    Console.Write(" " + nomeProduttore.ToUpper() + " ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write(" è gia presente in memoria.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
                }
            }
            SelezioneProduttore();

        }
        static public DBUser.IProduttore ResearchProduttore(string name)
        {
            DBUser.IProduttore pr;
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
        static public DBUser.IProduttore ResearchProduttore(int id)
        {
            DBUser.IProduttore pr;
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
        #endregion


        #region prodotto
        static void MenuProdotto()
        {
            #region display richiesta
            Console.Clear();
            Console.WriteLine($"Using Produttore {produttore.Nome}");
            using (var context = new DB.MagazzinoContext())
            {
                prodotti = context.Prodotti.Include(i => i.Storici).Where(i => i.ProduttoreId == produttore.Id).ToList();
            }
            Console.WriteLine("Digitare man per il manuale.");
            Console.WriteLine(hr);
            //Display costruttori su console
            int perRaw = 3;
            int raw = 0;
            foreach (var item in prodotti)
            {
                if (ricercaUltimoProdotto != null && item.CodiceArticolo.ToUpper().Contains(ricercaUltimoProdotto))
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.Write(String.Format("{0,3})", item.Id));
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(String.Format("{0,3}", item.Storici.Count));
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(String.Format("-{0,-25}", item.CodiceArticolo));
                if (ricercaUltimoProdotto != null && item.CodiceArticolo.ToUpper().Contains(ricercaUltimoProdotto))
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

            string input = Console.ReadLine().Trim();
            //ricerca comando
            List<string> args = input.Split(" ").ToList();
            if (args.Count == 1)
            {
                if (args[0] == "") args[0] = "man";
            }
            if (args.Count == 2)
            {
            }
            if (args.Count == 3)
            {
            }
            if (args.Count > 3)
            {
                if (args[0] == "") args[0] = "MostraErrore";
            }

            switch (args[0])
            {
                case "new":
                    NewProdotto(args[1]);
                    break;
                case "add":
                    AddProdotto(Convert.ToInt32(args[1]), Convert.ToInt32(args[2]));
                    break;
                case "remove":
                    RemoveProdotto(Convert.ToInt32(args[1]), Convert.ToInt32(args[2]));
                    break;
                case "search":
                    //SearchProdotto(args[1]);
                    break;
                case "return":
                    SelezioneProduttore();
                    break;
                case "man":
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Comandi validi :");
                    Console.WriteLine("add <numero_prodotto> <numero_elementi> [ aggiunta nuovi prodotti ]");
                    Console.WriteLine("remove <numero_prodotto> <numero_elementi> [ rimozione prodotti ]");
                    Console.WriteLine("search <nome_prdotto> [ ricerca prodotti ]");
                    Console.WriteLine("return [ ritorno al menu produttori ]");
                    Console.WriteLine("exit [ chiusura applicazione ]");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
                    MenuProdotto();
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                default:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Comando non valido.");
                    Console.WriteLine("Comandi validi :");
                    Console.WriteLine("add <numero_prodotto> <numero_elementi> [ aggiunta nuovi prodotti ]");
                    Console.WriteLine("remove <numero_prodotto> <numero_elementi> [ rimozione prodotti ]");
                    Console.WriteLine("search <nome_prdotto> [ ricerca prodotti ]");
                    Console.WriteLine("exit [ chiusura applicazione ]");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
                    Console.ReadLine();
                    MenuProdotto();
                    break;
            }

        }

        #region dbCalls
        public static void NewProdotto(string codice)
        {
            Magazzino.CreaNuovoProdotto(codice, produttore.Id);
            MenuProdotto();
        }
        public static void AddProdotto(int id, int numero)
        {
            Magazzino.AggiungiProdotto(id, numero);
            MenuProdotto();
        }
        public static void RemoveProdotto(int id, int numero)
        {

            using (var context = new DB.MagazzinoContext())
            {
                bool exist = context.Prodotti.Any(p => p.Id == id);
                if (exist)
                {
                    List<DB.Storico> s = context.Storici.Where(pr => pr.ProdottoId == id).Take(numero).ToList();
                    if (s != null) context.Storici.RemoveRange(s);

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
            MenuProdotto();
        }
        static public DBUser.IProdotto ResearchProdotto(string codiceArticolo)
        {
            return Magazzino.RicercaProdotto(codiceArticolo);
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
        #endregion



        #endregion
    }
}
