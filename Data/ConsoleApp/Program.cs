using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddDbContext<DB.MagazzinoContext>(options =>
            {
                var cnnStr = "Server= localhost;Database=MagazzinoGiDi;User Id=sa; Password=0000;";
                //var cnnStr = "Server=localhost;Database=_magazzino;Uid=pi;Pwd=0000;";
                //var cnnStr = "Server=192.168.1.7;Database=_magazzino;Uid=pi;Pwd=0000;";
                options.UseSqlServer(cnnStr);
            });
            services.AddTransient<App.IRunner,App.Runner>();
            var provider = services.BuildServiceProvider();

            provider.GetService<App.IRunner>().Start();
            Console.ReadLine();
        }
    }
}
