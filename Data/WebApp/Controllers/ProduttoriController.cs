using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ProduttoriController : Controller
    {
        private readonly DB.MagazzinoContext _context;

        public ProduttoriController(DB.MagazzinoContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Produttori.ToList());
        }

        public IActionResult NewProduttore(DB.Produttore produttore)
        {
            if (Request.Method == "POST" && produttore != null)
            {
                produttore.Nome = produttore.Nome.Trim().ToUpper();
                _context.AggiungiNuovoProduttore(produttore);
                return View("Index",_context.Produttori.ToList());
            }
            return View();
        }

        public IActionResult DisplayProduttore(int id)
        {
            return View(_context.Produttori.Single(i => i.Id == id));
        }

        public IActionResult SelezionaProduttore()
        {
            return View(_context.Produttori.ToList());
        }

        public IActionResult Modifiche(DB.Produttore pr)
        {
            _context.Update(pr);
            _context.SaveChanges();
            return View("DisplayProduttore", _context.Produttori.Single(i => i.Id == pr.Id));
        }

    }
}