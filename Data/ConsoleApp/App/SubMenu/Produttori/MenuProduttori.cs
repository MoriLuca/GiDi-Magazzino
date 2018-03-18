using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.App.SubMenu
{
    public class MenuProduttori
    {
        //public AppNodes SubMenuProduttori()
        //{
        //    int perRaw = 4;
        //    int raw = 0;

        //    Console.WriteLine(
        //           "Lista produttori registrati." + Environment.NewLine +
        //           "Scegliere un produttore per visualizzare i prodotti associati :");

        //    #region ScritturaProduttori
        //    foreach (var p in _magazzino.Produttori.ToList())
        //    {

        //        if (ricercaUltimoProduttore != null && p.Nome.ToUpper().Contains(ricercaUltimoProduttore))
        //        {
        //            Console.BackgroundColor = ConsoleColor.White;
        //            Console.ForegroundColor = ConsoleColor.Black;
        //        }
        //        Console.Write(String.Format("{0,3}){1,-15}", p.Id, p.Nome));
        //        if (ricercaUltimoProduttore != null && p.Nome.ToUpper().Contains(ricercaUltimoProduttore))
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
        //    #endregion

        //    _consoleDecorator.AppPlaceholder("Gi.Di.Magazzino");
        //    _commandParser.InputString = Console.ReadLine().Trim().ToLower();
        //    switch (_commandParser.CommandPack.Command)
        //    {
        //        case "1":
        //        case "menu":
        //            return AppNodes.MainMenu;
        //        case "2":
        //        case "magazzino":
        //            return AppNodes.GestioneMagazzino;
        //        case "3":
        //        case "produttori":
        //            return AppNodes.ListaProduttori;
        //        case "4":
        //        case "prodotti":
        //            return AppNodes.ListaProdotti;
        //        case "exit":
        //        case "quit":
        //        case "end":
        //            return AppNodes.ExitApplication;
        //        default:
        //            return AppNodes.InputError;
        //    }
        //}
        public void Start()
        {
            string input = null;
            while(input != "exit")
            {
                Console.Write("Inserire comando :");
                input = Console.ReadLine();
            }
        }
    }
}
