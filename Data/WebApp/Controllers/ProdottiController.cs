using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ProdottiController : Controller
    {
        private readonly DB.MagazzinoContext _context;

        public ProdottiController(DB.MagazzinoContext context)
        {
            _context = context;
        }

        public IActionResult ProductOperation(int id, bool operation, int number)
        {
            if(Request.Method == "POST")
            {
                if (!operation) _context.RimuoviProdotto(id, number);
                if (operation) _context.AggiungiProdotto(id, number);
                List<DB.Prodotto> p = new List<DB.Prodotto>();
                p.Add(_context.RicercaProdotto(id));
                return View("../Home/Index",p);
            }
            return View("Index");
        }

        public IActionResult NuovoProdotto(int id, bool operation, int number)
        {
            if (Request.Method == "POST")
            {
                if (!operation) _context.RimuoviProdotto(id, number);
                if (operation) _context.AggiungiProdotto(id, number);
                List<DB.Prodotto> p = new List<DB.Prodotto>();
                p.Add(_context.RicercaProdotto(id));
                return View("../Home/Index", p);
            }
            return View();
        }
    }
}