using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.PomocneMetode
{
    public static class NasumicnaLoza
    {
        private static readonly Random random = new Random();
        public static string GenerisiNasumicnaLoza()
        {
            var loze = new List<string>
            {
                "Merlot",
                "Cabernet Sauvignon",
                "Chardonnay",
                "Pinot Noir",
                "Sauvignon Blanc"
            };

            return loze[random.Next(loze.Count)];
        }
    }
}
