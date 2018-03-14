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

        static List<DB.Produttore> _produttori = new List<DB.Produttore>();
        static List<DB.Prodotto> _prodotti = new List<DB.Prodotto>();
        static DB.Produttore _produttore = new DB.Produttore();
        static DB.Prodotto _prodotto = new DB.Prodotto();
        static Stage stage = Stage.MainMenu;


        private enum Stage
        {
            MainMenu,
            SelezioneProduttore,
            MenuProdotti,
            RicercaPerCodice
        }
        #endregion

        static void Main(string[] args)
        {
            while (true)
            {
                switch (stage)
                {
                    case Stage.MainMenu:
                        MainMenu();
                        break;
                    case Stage.SelezioneProduttore:
                        SelezioneProduttore();
                        break;
                    case Stage.MenuProdotti:
                        MenuProdotto();
                        break;
                    case Stage.RicercaPerCodice:
                        RicercaPerCodice();
                        break;
                }
            }
        }


        /// <summary>
        /// Ricerca produttori per nome
        /// </summary>
        /// <param name="nomeProduttore"></param>
        public static void SearchProduttore(string nomeProduttore)
        {

            using (var context = new DB.MagazzinoContext())
            {
                int n = Magazzino.GetProduttori(nomeProduttore).Count;
                if (n > 0)
                {
                    ricercaUltimoProduttore = nomeProduttore.Trim().ToUpper();
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
        }
        /// <summary>
        /// Inserire un nuovo produttore
        /// </summary>
        /// <param name="nomeProduttore"></param>
        public static void InsertNewProduttore(string nomeProduttore)
        {
            Magazzino.AggiungiNuovoProduttore(new DB.Produttore()
            {
                Nome = nomeProduttore.Trim().ToUpper(),
            });
        }
        /// <summary>
        /// Restituisce un produttore in base all'id fornito
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static public DB.Produttore GetProduttore(int id)
        {
            return Magazzino.GetProduttore(id);
        }



        public static void NewProdotto(string codice, int pezzi = 0)
        {
            int[] raws = Magazzino.CreaNuovoProdotto(codice, _produttore.Id, pezzi);
            Console.WriteLine(raws[0].ToString(), raws[1]);
        }
        public static void AddProdotto(string id, int numero)
        {
            Int16 i;
            int raws;
            if (Int16.TryParse(id, out i))
            {
                raws = Magazzino.AggiungiProdotto(i, numero);
            }
            else
            {
                raws = Magazzino.AggiungiProdotto(id, numero);
            }

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($"Aggiornate {raws} righe sul database.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
        }
        public static void RemoveProdotto(int id, int numero)
        {
            int raws = Magazzino.RimuoviProdotto(id, numero);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($"Aggiornate {raws} righe sul database.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
        }
        static public DB.Prodotto ResearchProdotto(string codiceArticolo)
        {
            return Magazzino.RicercaProdotto(codiceArticolo);
        }
        static public DB.Prodotto ResearchProdotto(int id)
        {
            return Magazzino.RicercaProdotto(id);
        }



        /// <summary>
        /// Stage Main Menu
        /// </summary>
        static void MainMenu()
        {
            Console.WriteLine(hr);
            Console.WriteLine("Menu : ");
            Console.WriteLine("1) Ricerca per codice prodotto");
            Console.WriteLine("2) Gestione Archivio"+Environment.NewLine);
            Console.WriteLine("'exit' per chiudere l'applicazione");
            Console.WriteLine(hr);

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    stage = Stage.RicercaPerCodice;
                    break;
                case "2":
                    stage = Stage.SelezioneProduttore;
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                case "clear":
                    Console.Clear();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Comando non valido.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
                    break;
            }

        }

        /// <summary>
        /// Stage Selezione produttore
        /// </summary>
        static void SelezioneProduttore()
        {
            _produttori = Magazzino.GetProduttori();

            Console.WriteLine("Digitare man per il manuale.");
            Console.WriteLine(hr);
            //Display costruttori su console
            int perRaw = 4;
            int raw = 0;
            foreach (var item in _produttori)
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



            string input = Console.ReadLine().Trim();

            if (int.TryParse(input, out number))
            {
                _produttore = GetProduttore(number);
                if (_produttore == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Inserito un numero produttore non valido.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
                    SelezioneProduttore();
                }
                else stage = Stage.MenuProdotti;
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
                    case "new":
                        InsertNewProduttore(argument);
                        break;
                    case "search":
                        SearchProduttore(argument);
                        break;
                    case "man":
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Comandi validi :");
                        Console.WriteLine("new < nome_produttore > [ aggiungere un nuovo produttore ]");
                        Console.WriteLine("search < nome_produttore > [ ricerca produttori ]");
                        Console.WriteLine("exit [ chiusura applicazione ]");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ReadLine();
                        break;
                    case "exit":
                        Environment.Exit(0);
                        break;
                    case "clear":
                        Console.Clear();
                        break;
                    case "return":
                    case "menu":
                    case "main":
                        stage = Stage.MainMenu;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Comando non valido.");
                        Console.WriteLine("Comandi validi :");
                        Console.WriteLine("new < nome_produttore > [ aggiungere un nuovo produttore ]");
                        Console.WriteLine("search < nome_produttore > [ ricerca produttori ]");
                        Console.WriteLine("exit [ chiusura applicazione ]");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ReadLine();
                        break;
                }

            }



        }
        /// <summary>
        /// Stage menu prodotti
        /// </summary>
        static void MenuProdotto()
        {
            Console.WriteLine($"Using Produttore {_produttore.Nome}");
            using (var context = new DB.MagazzinoContext())
            {
                _prodotti = context.Prodotti.Include(i => i.Storici).Where(i => i.ProduttoreId == _produttore.Id).ToList();
            }
            Console.WriteLine("Digitare man per il manuale.");
            Console.WriteLine(hr);
            //Display costruttori su console
            int perRaw = 3;
            int raw = 0;
            foreach (var item in _prodotti)
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
                    if (args.Count == 3) NewProdotto(args[1], Convert.ToInt16(args[2]));
                    if (args.Count == 2) NewProdotto(args[1]);
                    break;
                case "add":
                    AddProdotto((args[1]), Convert.ToInt32(args[2]));
                    break;
                case "remove":
                    RemoveProdotto(Convert.ToInt32(args[1]), Convert.ToInt32(args[2]));
                    break;
                case "search":
                    //SearchProdotto(args[1]);
                    break;
                case "return":
                    stage = Stage.SelezioneProduttore;
                    break;
                case "clear":
                    Console.Clear();
                    break;
                case "main":
                case "menu":
                    stage = Stage.MainMenu;
                    break;
                case "man":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Comandi validi :");
                    Console.WriteLine("add <numero_prodotto> <numero_elementi> [ aggiunta nuovi prodotti ]");
                    Console.WriteLine("remove <numero_prodotto> <numero_elementi> [ rimozione prodotti ]");
                    Console.WriteLine("search <nome_prdotto> [ ricerca prodotti ]");
                    Console.WriteLine("return [ ritorno al menu produttori ]");
                    Console.WriteLine("exit [ chiusura applicazione ]");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                default:
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
                    break;
            }

        }
        /// <summary>
        /// Stage Ricerca Prodotto per codice
        /// </summary>
        static void RicercaPerCodice()
        {
            Console.Write(gidi+"Digitare il codice da ricercare : ");

            string input = Console.ReadLine().Trim();
            //ricerca comando

            switch (input)
            {
                case "return":
                case "main":
                case "menu":
                    stage = Stage.MainMenu;
                    break;
                case "man":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Inserire il codice prodotto da ricercare");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                case "clear":
                    Console.Clear();
                    break;
                default:
                    _prodotti = Magazzino.GetProdotti(input.Trim().ToUpper());
                    if (_prodotti.Count > 0)
                    {
                        int perRaw = 1;
                        int raw = 0;
                        foreach (var item in _prodotti)
                        {
                            Console.Write(String.Format("{0,3})", item.Id));
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write(String.Format("{0,3}", item.Storici.Count));
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(String.Format("-{0,-25}", item.CodiceArticolo));
                            Console.Write(String.Format("-{0,-25}", item.Produttore.Nome));

                            raw++;
                            if (raw == perRaw)
                            {
                                Console.WriteLine();
                                raw = 0;
                            }
                        }
                        if (raw < perRaw) Console.WriteLine();
                        Console.WriteLine(hr);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Nessun prodotto trovato.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                   break;
            }

        }
    }
}
