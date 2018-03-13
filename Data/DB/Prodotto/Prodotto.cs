using System;
using System.Collections.Generic;
using System.Text;

namespace DB
{
    public class Prodotto
    {
        public int Id { get; set; }
        public int CodiceArticolo { get; set; }
        public Produttore Produttore { get; set; }
    }
}
