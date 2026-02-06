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
        public string Id { get; set; } = "";
        public TipProdaje TipProdaje { get; set; } = 0;

        public NacinPlacanja NacinPlacanja { get; set; } = 0;
        public List<string> id_vina { get; set; } = new List<string>();
        public int Kolicina { get; set; } = 0;

        public float UkupanIznos { get; set; } = 0;
        public DateTime DatumKreiranja { get;  set; } = DateTime.UtcNow;

        public Faktura() { }
        public Faktura(TipProdaje tipProdaje, NacinPlacanja nacinPlacanja, List<string> stavke, float ukupan_iznos, int ukupno_flasa)
        {
            Id = Guid.NewGuid().ToString();
            TipProdaje = tipProdaje;
            NacinPlacanja = nacinPlacanja;
            id_vina = stavke;
            UkupanIznos = ukupan_iznos;
            DatumKreiranja = DateTime.UtcNow;
            Kolicina = ukupno_flasa;
        }

        public string Header()
        {
            return $@"| {"ID",-10} | {"TIP PRODAJE",-20} | {"NACIN PLACANJA",-20} | {"UKUPAN IZNOS",-15}" +
                   "\n--------------------------------------------------------------------------------------------------------------------\n";
        }

        public override string ToString()
        {
            return $@"| {Id,-10} | {TipProdaje,-20} | {NacinPlacanja,-20} | {UkupanIznos,-15}";
        }
    }
}