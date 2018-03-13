using System;
using System.Collections.Generic;
using System.Text;

namespace DB
{
    class Magazzino
    {
        public int Id { get; set; }
        public Prodotto Prodotto { get; set; }
        public DateTime DataInserimento { get; set; }
        public DateTime DataRitiro { get; set; }
        public Stato Stato { get; set; }
        public Scopo Scopo { get; set; }
    }
}
