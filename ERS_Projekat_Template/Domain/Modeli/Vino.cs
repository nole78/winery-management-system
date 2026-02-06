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

        public string Header()
        {
            return $@"| {"ID",-10} | {"NAZIV",-20} | {"TIP",-20} | {"ZAPREMINA",-5} | {"SIFRA SERIJE", -20} | {"ID_LOZE", -5} | {"DATUM FLASIRANJA", -20}" +
                   "\n--------------------------------------------------------------------------------------------------------------------\n";
        }

        public override string ToString()
        {
            return $@"| {ID_VINA,-10} | {Naziv,-20} | {Tip,-20} | {Zapremina,-5} | {SifraSerije,-20} | {IdLoze,-5} | {DatumFlasiranja,-20}";
        }

    }
}
