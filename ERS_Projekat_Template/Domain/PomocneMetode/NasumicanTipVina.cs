using Domain.Enumeracije;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.PomocneMetode
{
    public static class NasumicanTipVina
    {
        private static readonly Random random = new Random();

        public static TipVina GenerisiNasumicanTipVina()
        {
            var values = Enum.GetValues(typeof(TipVina));
            return (TipVina)values.GetValue(random.Next(values.Length));
        }
    }
}
