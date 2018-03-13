using System.Collections.Generic;
using DBUser;

namespace DB
{
    public class Produttore : DBUser.IProduttore
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string NumeroTelefono { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public List<IProdotto> Prodotti { get; set; }
    }
}
