using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace UiConsole
{
    class Program
    {
        //static DB._magazzinoContext _magazzino = new DB._magazzinoContext(new DbContextOptionsBuilder());
        #region Properties
        static string gidi = "_magazzinoGiDi>";
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
            // configure services
            var services = new ServiceCollection();
                //.AddDbContext<DB._magazzinoContext>(options =>
                //{
                //    var cnnStr = "Server= localhost;Database=_magazzinoGiDi;User Id=sa; Password=0000;";
                //    //var cnnStr = "Server=localhost;Database=_magazzino;Uid=pi;Pwd=0000;";
                //    //var cnnStr = "Server=192.168.1.7;Database=_magazzino;Uid=pi;Pwd=0000;";
                //    options.UseSqlServer(cnnStr);
                //});
            services.AddTransient<DB.IMagazzino,DB.MagazzinoContext>();
            services.AddTransient<DBController>();
            var provider = services.BuildServiceProvider();
            while (true)
            {
                switch (stage)
                {
                    case Stage.MainMenu:
                        MainMenu();
                        break;
                    case Stage.SelezioneProduttore:
                        
                        break;
                    //case Stage.MenuProdotti:
                    //    MenuProdotto();
                    //    break;
                    //case Stage.RicercaPerCodice:
                    //    //RicercaPerCodice();
                    //    break;
                }
            }
        }
        /// <summary>
        /// Stage Main Menu
        /// </summary>
        public static  void MainMenu()
        {
            Console.WriteLine(hr);
            Console.WriteLine("Menu : ");
            Console.WriteLine("1) Ricerca per codice prodotto");
            Console.WriteLine("2) Gestione Archivio" + Environment.NewLine);
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
        //public void SelezioneProduttore()
        //{
        //    Console.WriteLine("Digitare man per il manuale.");
        //    Console.WriteLine(hr);
        //    //Display costruttori su console
        //    int perRaw = 4;
        //    int raw = 0;
        //    foreach (var item in _produttori)
        //    {
        //        if (ricercaUltimoProduttore != null && item.Nome.ToUpper().Contains(ricercaUltimoProduttore))
        //        {
        //            Console.BackgroundColor = ConsoleColor.White;
        //            Console.ForegroundColor = ConsoleColor.Black;
        //        }
        //        Console.Write(String.Format("{0,3}){1,-15}", item.Id, item.Nome));
        //        if (ricercaUltimoProduttore != null && item.Nome.ToUpper().Contains(ricercaUltimoProduttore))
        //        {
        //            Console.BackgroundColor = ConsoleColor.Black;
        //            Console.ForegroundColor = ConsoleColor.White;
        //        }
        //        raw++;
        //        if (raw == perRaw)
        //        {
        //            Console.WriteLine();
        //            raw = 0;
        //        }
        //    }
        //    if (raw < perRaw) Console.WriteLine();
        //    Console.WriteLine(hr);
        //    Console.Write(gidi);



        //    string input = Console.ReadLine().Trim();

        //    if (int.TryParse(input, out number))
        //    {
        //        _produttore = providers GetProduttore(number);
        //        if (_produttore == null)
        //        {
        //            Console.ForegroundColor = ConsoleColor.DarkYellow;
        //            Console.WriteLine("Inserito un numero produttore non valido.");
        //            Console.ForegroundColor = ConsoleColor.White;
        //            Console.ReadLine();
        //            SelezioneProduttore();
        //        }
        //        else stage = Stage.MenuProdotti;
        //    }
        //    else
        //    {

        //        //ricerca comando
        //        int comandLenght = input.IndexOf(" ");
        //        if (comandLenght == -1) input += " ";
        //        comandLenght = input.IndexOf(" ");
        //        string comando = input.Substring(0, comandLenght);
        //        string argument = input.Substring(input.IndexOf(" "));

        //        switch (comando)
        //        {
        //            case "new":
        //                InsertNewProduttore(argument);
        //                break;
        //            case "search":
        //                SearchProduttore(argument);
        //                break;
        //            case "man":
        //                Console.ForegroundColor = ConsoleColor.DarkYellow;
        //                Console.WriteLine("Comandi validi :");
        //                Console.WriteLine("new < nome_produttore > [ aggiungere un nuovo produttore ]");
        //                Console.WriteLine("search < nome_produttore > [ ricerca produttori ]");
        //                Console.WriteLine("exit [ chiusura applicazione ]");
        //                Console.ForegroundColor = ConsoleColor.White;
        //                Console.ReadLine();
        //                break;
        //            case "exit":
        //                Environment.Exit(0);
        //                break;
        //            case "clear":
        //                Console.Clear();
        //                break;
        //            case "return":
        //            case "menu":
        //            case "main":
        //                stage = Stage.MainMenu;
        //                break;
        //            default:
        //                Console.ForegroundColor = ConsoleColor.DarkYellow;
        //                Console.WriteLine("Comando non valido.");
        //                Console.WriteLine("Comandi validi :");
        //                Console.WriteLine("new < nome_produttore > [ aggiungere un nuovo produttore ]");
        //                Console.WriteLine("search < nome_produttore > [ ricerca produttori ]");
        //                Console.WriteLine("exit [ chiusura applicazione ]");
        //                Console.ForegroundColor = ConsoleColor.White;
        //                Console.ReadLine();
        //                break;
        //        }

        //    }



        //}

    }
}
