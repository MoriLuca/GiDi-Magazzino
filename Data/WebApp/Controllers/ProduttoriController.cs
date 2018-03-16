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

        public IActionResult NewProduttore(DB.Produttore produttore)
        {
            if (Request.Method == "POST")
            {
                _context.AggiungiNuovoProduttore(produttore);
                return View(@"..\Home\Index");
            }
            return View();
        }
    }
}