using System.Collections.Generic;

namespace DB
{
    public class Produttore
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string NumeroTelefono { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public List<Prodotto> Prodotti { get; set; }
    }
}
