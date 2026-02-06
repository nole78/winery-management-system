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

        public string ID_VINA { get; set; } = string.Empty;
        public TipVina Tip { get; set; }  = TipVina.STOLNO;
        public double Zapremina { get; set; } = 0; // u litrima


      
        public string SifraSerije { get; set; } = string.Empty;

        public string IdLoze { get; set; } = string.Empty;
        public DateTime DatumFlasiranja { get; set; } = DateTime.Now;

        public Vino() {}

        public Vino(string id, string naziv, TipVina t, double zapremina, string idloze, DateTime df)
        {
            ID_VINA = id;
            Naziv = naziv;
            Tip = t;
            Zapremina = zapremina;
            IdLoze = idloze;
            DatumFlasiranja = df;
        }

    

    }
}
