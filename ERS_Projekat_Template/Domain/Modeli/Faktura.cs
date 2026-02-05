using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;




namespace Domain.Modeli
{
    public class Faktura
    {
        public Guid Id { get;  set; }
        public TipProdaje TipProdaje { get;  set; }
        public NacinPlacanja NacinPlacanja { get;  set; }
        public List<StavkaFakture> Stavke { get;  set; }
        public decimal UkupanIznos { get;  set; }
        public DateTime DatumKreiranja { get;  set; }

        public Faktura() {
           
        }

        public static Faktura Kreiraj(TipProdaje tipProdaje, NacinPlacanja nacinPlacanja, List<StavkaFakture> stavke)
        {
            return new Faktura
            {
                Id = Guid.NewGuid(),
                TipProdaje = tipProdaje,
                NacinPlacanja = nacinPlacanja,
                Stavke = stavke,
                UkupanIznos = stavke.Sum(s => s.JedinicnaCena * s.Kolicina),
                DatumKreiranja = DateTime.UtcNow
            };
        }


    }
}