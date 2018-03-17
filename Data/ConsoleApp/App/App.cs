using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp.App
{
    class Runner : IRunner
    {
        private readonly DB.MagazzinoContext _magazzino;

        public Runner(DB.MagazzinoContext magazzino)
        {
            _magazzino = magazzino;
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            foreach (var pr in _magazzino.Produttori.ToList())
            {
                Console.WriteLine(pr.Nome);
            }
            Console.WriteLine("Im starting the application right now!");
            _magazzino.Produttori.Add(new DB.Produttore() { Nome = "baldoria"});
            
            Console.WriteLine($"Ho salvato {_magazzino.SaveChanges()} righe");
        }
    }

    class RunnerIno : IRunner
    {
        public void Exit()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            Console.WriteLine("Im starting the application right now! I'M runnerino by the way !");
        }
    }

    public interface IRunner
    {
        void Start();
        void Exit();
    }
}
