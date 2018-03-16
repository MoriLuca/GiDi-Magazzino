using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly DB.MagazzinoContext _context;

        public HomeController(DB.MagazzinoContext context)
        {
            _context = context;
        }

        public IActionResult Index(string codice)
        {
            if (Request.Method == "GET" && codice != null)
            {
                try
                {
                    codice = codice.Trim().ToUpper();
                    List<DB.Prodotto> prodotti = _context.GetProdotti(codice);
                    if (prodotti.Count > 0) return View(prodotti);
                    //se il codice non è presente nell'archivio 
                    else
                    {
                        ViewData["creazioneNuovoProdotto"] = true;
                        ViewData["codiceNonTrovato"] = codice;
                        return View(@"..\Produttori\SelezionaProduttore",
                            _context.GetProduttori());
                    }
                }
                catch (Exception ex)
                {
                    ViewData["ex"] = ex.Message;
                }

            }
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
