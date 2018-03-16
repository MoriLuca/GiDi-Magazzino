using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp
{
    public class Startup
    {
        public IConfiguration _config { get; }

        public Startup(IHostingEnvironment env)
        {
            //Leggo i parametri
            //creo due file di configurazione, il primo di default, il secondo, opzionale (true), per sovrascrivere
            //i valori che interessano, per esempio in fase di debug
            //_config = new ConfigurationBuilder()
            //                        .AddEnvironmentVariables()
            //                        .AddJsonFile(env.ContentRootPath + "/appsettings.json")
            //                        .AddJsonFile(env.ContentRootPath + "/appsettings.Development.json", true)
            //                        .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<DB.MagazzinoContext>();
            ////Da utilizzare per la release, per il debug utilizzo il costruttore senza parametri
            ////perchè utilizzo la definizione all'interno del costruttore
            //services.AddDbContext<DB.MagazzinoContext>(options =>
            //{
            //    var cnnStr = _config.GetValue<string>("ConnectionString:MagazzinoDev");
            //    options.UseSqlServer(cnnStr);
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
