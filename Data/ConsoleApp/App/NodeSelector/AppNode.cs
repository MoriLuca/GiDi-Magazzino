using System;

namespace ConsoleApp.App
{
    public enum AppNodes
    {
        InputError,
        MainMenu,
        GestioneMagazzino,
        ListaProduttori,
        ListaProdotti,
        ExitApplication
    }
    public class NodeSelector : INodeSelector
    {
        private readonly ICommandParser _commandParser;
        private readonly IConsoleDecorator _consoleDecorator;

        public NodeSelector(ICommandParser commandParser, IConsoleDecorator consoleDecorator)
        {
            _commandParser = commandParser;
            _consoleDecorator = consoleDecorator;
        }
        public AppNodes SelectNode()
        {
            Console.WriteLine(
                "Opzioni Menù :" + Environment.NewLine +
                "1) Menu" + Environment.NewLine +
                "2) Magazzino" + Environment.NewLine +
                "3) Prodotti" + Environment.NewLine +
                "4) Produttori" + Environment.NewLine +
                "0) Exit" + Environment.NewLine +
                "" + Environment.NewLine +
                "Esempio Utilizzo :" + Environment.NewLine +
                "Per eseguire un comando, inserire il numero corrispondente." + Environment.NewLine +
                "Altrimenti è possibile scrivere il nome del sottomenù." + Environment.NewLine +
                "Es : 4 || produttori -> porteranno al sottomenu Produttori." + Environment.NewLine);

            _consoleDecorator.AppPlaceholder("Gi.Di.Magazzino");
            _commandParser.InputString = Console.ReadLine().Trim().ToLower();
            switch (_commandParser.CommandPack.Command)
            {
                case "1":
                case "menu":
                    return AppNodes.MainMenu;
                case "2":
                case "magazzino":
                    return AppNodes.GestioneMagazzino;
                case "3":
                case "produttori":
                    return AppNodes.ListaProduttori;
                case "4":
                case "prodotti":
                    return AppNodes.ListaProdotti;
                case "exit":
                case "quit":
                case "end":
                    return AppNodes.ExitApplication;
                default:
                    return AppNodes.InputError;
            }
        }
    }

    public interface INodeSelector
    {
        AppNodes SelectNode();
    }

}
