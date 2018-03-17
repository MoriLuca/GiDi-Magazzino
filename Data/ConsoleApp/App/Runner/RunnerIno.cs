using System;

namespace ConsoleApp.App
{
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

}