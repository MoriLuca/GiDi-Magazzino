using System;

namespace ConsoleApp.App
{
    class Runner : IRunner
    {
        #region services
        
        private readonly App.INodeSelector _nodeSelector;
        private readonly App.IConsoleDecorator _consoleDecorator;
        #endregion

        #region property
        #endregion


        public Runner(INodeSelector nodeSelector,
                      IConsoleDecorator consoleDecorator )
        {
            _nodeSelector = nodeSelector;
            _consoleDecorator = consoleDecorator;
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            Console.WindowWidth = 200;
            Console.WindowHeight = 50;
            Console.Title = $"Magazzino Gi.Di. Automazione V[{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}]";
            while (true)
            {
                switch (_nodeSelector.MainMenu())
                {
                    case AppNodes.GestioneMagazzino:
                        Console.WriteLine("gestione magazzino");
                        break;
                    case AppNodes.ListaProduttori:
                        new App.SubMenu.MenuProduttori().Start();
                        break;
                    case AppNodes.ListaProdotti:
                        Console.WriteLine("lista prodotti");
                        break;
                    case AppNodes.InputError:
                        Console.WriteLine("errore codice immesso");
                        break;
                    case AppNodes.ExitApplication:
                        _consoleDecorator.BrakeLine(ConsoleColor.DarkYellow);
                        Console.WriteLine("Grazie per aver utilizzato il magazzino Gidi, a presto!");
                        Console.ReadLine();
                        Environment.Exit(0);
                        break;
                }
            }
        }

    }

}