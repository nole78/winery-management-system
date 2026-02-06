using Domain.Enumeracije;
using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.PomocneMetode
{
    public static class RacunanjeCene
    {
        public static float IzracunajCenu(Vino vino)
        {
            return vino.Tip switch
            {
                TipVina.STOLNO => 800,
                TipVina.KVALITETNO => 1400,
                TipVina.PREMIJUM => 2500,
                _ => 1000
            };
        }
    }
}
