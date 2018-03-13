using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DB
{
    public class Prodotto
    {
        public int Id { get; set; }
        public string CodiceArticolo { get; set; }
        public int ProduttoreId { get; set; }
        public Produttore Produttore { get; set; }
        public Decimal PrezzoAcquisto { get; set; }
        public Decimal PrezzoVendita { get; set; }

        List<Storico> Storici { get; set; }
    }
}
