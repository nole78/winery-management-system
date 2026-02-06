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
        public string Id { get;  set; }
        public TipProdaje TipProdaje { get;  set; }
        public NacinPlacanja NacinPlacanja { get; set; }
        
        public List<Vino> SpisakVina { get; set; }

        public int Kolicina { get; set; }   

        public float UkupanIznos { get; set; } = 0;
        public DateTime DatumKreiranja { get;  set; }

        public Faktura() {
           
        }

        public static Faktura Kreiraj(TipProdaje tipProdaje, NacinPlacanja nacinPlacanja, int Kolicina)
        {
            return new Faktura
            {

                TipProdaje = tipProdaje,
                NacinPlacanja = nacinPlacanja,
                Kolicina = Kolicina,
                SpisakVina = new List<Vino>(),
                UkupanIznos = 0,
                DatumKreiranja = DateTime.Now
            };
        }


    }
}