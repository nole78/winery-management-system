using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.PomocneMetode.NasumicneVrednosti
{
    public static class NasumicnaZapreminaFlase
    {
        private static readonly Random random = new();
        public static double GenerisiNasumicnaZapreminaFlase()
        {
            return random.Next(2) == 0 ? 0.75 : 1.5;
        }
    }
}
