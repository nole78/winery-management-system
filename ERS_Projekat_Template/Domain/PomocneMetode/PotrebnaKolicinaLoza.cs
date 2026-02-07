using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Konstante;

namespace Domain.PomocneMetode
{
    public static class PotrebnaKolicinaLoza
    {
        public static int IzracunajPotrebnuKolicinuLoza(int brojFlasa, double zapreminaFlase) // pomocna
        {
            double ukupno = brojFlasa * zapreminaFlase;
            return (int)Math.Ceiling(ukupno / KolicinaVinaPoLozi.LITARA_PO_LOZI);
        }

    }
}
