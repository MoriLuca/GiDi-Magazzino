using System;
using System.Collections.Generic;

namespace DBUser
{
    public interface IMagazzinoContext
    {
        void CreaNuovoProdotto(string codice, int produttoreId, decimal costoAquisto = 0, decimal prezzoVendita = 0);
        void AggiungiProdotto(int id, int numero);
        IProdotto RicercaProdotto(string codiceProdotto);
        List<IProduttore> GetProduttori();
    }

    public interface IProdotto
    {
        int Id { get; set; }
        string CodiceArticolo { get; set; }
        int ProduttoreId { get; set; }
        IProduttore Produttore { get; set; }
        List<IStorico> Storici { get; set; }
    }

    public interface IProduttore
    {
        int Id { get; set; }
        string Nome { get; set; }
        List<IProdotto> Prodotti { get; set; }
    }

    public interface IStorico
    {
        int Id { get; set; }
        int ProdottoId { get; set; }
        IProdotto Prodotto { get; set; }
        DateTime DataInserimento { get; set; }
        DateTime DataRitiro { get; set; }
    }
}
