using System;
using System.Collections.Generic;
using System.Text;
using DBUser;

namespace DB
{
    public class Storico : DBUser.IStorico
    {
        public int Id { get; set; }
        public int ProdottoId { get; set; }
        public IProdotto Prodotto { get; set; }
        public DateTime DataInserimento { get; set; }
        public DateTime DataRitiro { get; set; }
        public Stato Stato { get; set; }
        public Scopo Scopo { get; set; }
    }
}
