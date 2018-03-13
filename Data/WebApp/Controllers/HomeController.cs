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
        private readonly Data.MagazzinoContext _context;

        public HomeController(Data.MagazzinoContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetItems()
        {
            return View(_context.Prodotti.ToList());
        }

        public IActionResult NewItem(Data.Prodotto_Base prodotto)
        { 
            if (Request.Method == "POST")
            {
                try
                {
                    if (prodotto.CodiceProdotto == null) throw new Exception("Codice prodotto cannot be Null.");
                    prodotto.CodiceProdotto = prodotto.CodiceProdotto.Trim().ToUpper();
                    _context.Prodotti_Base.Add(prodotto);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ViewData["ex"] = ex.Message ;
                }
                
            }
            return View(prodotto);
        }

        [HttpPost]
        public JsonResult CodiceProdotto(string txt)
        {
            //Searching records from list using LINQ query  
            var list = _context.Prodotti_Base.Where(c => c.CodiceProdotto.Contains(txt)).ToList();
            return Json(list);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
