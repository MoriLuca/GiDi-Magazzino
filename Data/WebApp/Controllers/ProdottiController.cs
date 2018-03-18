using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers
{

    public class ProdottiController : Controller
    {
        private readonly DB.MagazzinoContext _context;

        public ProdottiController(DB.MagazzinoContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<DB.Prodotto> p = _context.GetProdotti();
            return View(_context.GetProdotti());
        }

        public IActionResult Search(string name)
        {
            List<DB.Prodotto> pr = _context.GetProdotti(name);
            if (pr.Count == 1) return View("SearchFound", pr[0]);
            else return View(pr);
        }

        public IActionResult ProductOperation(int id, bool operation, int number)
        {
            if(Request.Method == "POST")
            {
                if (!operation) _context.RimuoviProdotto(id, number);
                if (operation) _context.AggiungiProdotto(id, number);
                List<DB.Prodotto> p = new List<DB.Prodotto>();
                p.Add(_context.RicercaProdotto(id));
                return View(@"..\Home\Index",p);
            }
            return View("Index");
        }

        public IActionResult NuovoProdotto(int idProduttore)
        {
            if (Request.Method == "POST")
            {
                DB.Prodotto p = new DB.Prodotto()
                {
                    ProduttoreId = idProduttore,
                };
                return View(@"..\Prodotti\NuovoProdotto", p);
            }
            return View();
        }
        public IActionResult SalvaNuovoProdotto(DB.Prodotto prodotto)
        {
            _context.CreaNuovoProdotto(prodotto);
            List<DB.Prodotto> p = new List<DB.Prodotto>();
            p = _context.GetProdotti(prodotto.CodiceArticolo);
            return View(@"..\Home\Index", p);
        }

    }
}