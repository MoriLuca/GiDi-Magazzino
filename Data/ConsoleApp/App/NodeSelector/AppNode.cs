using System;
using System.Linq;

namespace ConsoleApp.App
{
    public enum AppNodes
    {
        InputError,
        GestioneMagazzino,
        ListaProduttori,
        ListaProdotti,
        ExitApplication
    }
    public class NodeSelector : INodeSelector
    {
        private readonly ICommandParser _commandParser;
        private readonly IConsoleDecorator _consoleDecorator;

        public NodeSelector(ICommandParser commandParser,
            IConsoleDecorator consoleDecorator)
        {
            _commandParser = commandParser;
            _consoleDecorator = consoleDecorator;
        }

        public AppNodes MainMenu()
        {
            _consoleDecorator.DrawLogo("GiDi.txt");
            Console.WriteLine(
                "Opzioni Menù :" + Environment.NewLine +
                "1) Magazzino" + Environment.NewLine +
                "2) Prodotti" + Environment.NewLine +
                "3) Produttori" + Environment.NewLine +
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
                case "magazzino":
                    return AppNodes.GestioneMagazzino;
                case "2":
                case "prodotti":
                    return AppNodes.ListaProduttori;
                case "3":
                case "produttori":
                    return AppNodes.ListaProdotti;
                case "0":
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
        AppNodes MainMenu();
    }

}
