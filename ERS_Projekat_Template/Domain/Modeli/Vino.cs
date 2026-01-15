using Domain.Enumeracije;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Modeli
{
    public class Vino
    {
        public string Naziv { get; set; } = string.Empty;

        public string ID_VINA { get; set; }
        public TipVina Tip { get; set; }  
        public double Zapremina { get; set; } = 0; // u litrima


      
        public string SifraSerije { get; set; }

        public string IdLoze { get; set; } = string.Empty;
        public DateTime DatumFlasiranja { get; set; } = DateTime.Now;

        public Vino() {
            ID_VINA = Guid.NewGuid().ToString();
            SifraSerije = "VN-2025-" + ID_VINA;
        }

        public Vino(string naziv, TipVina t, double zapremina, string idloze, DateTime df)
        {
            Naziv = naziv;
            Tip = t;
            Zapremina = zapremina;
            ID_VINA = Guid.NewGuid().ToString();
            SifraSerije = "VN-2025-" + ID_VINA;
            IdLoze = idloze;
            DatumFlasiranja = df;
        }

    

    }
}
