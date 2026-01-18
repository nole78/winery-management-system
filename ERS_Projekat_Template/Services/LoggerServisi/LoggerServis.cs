using Domain.Enumeracije;
using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.LoggerServisi
{
    public class LoggerServis : ILoggerServis
    {
        private string putanja;

        public bool EvidentirajDogadjaj(TipEvidencije tip, string poruka)
        {
            try
            {
                string datum = DateTime.Now.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
                using StreamWriter sw = new(putanja, append: true);
                sw.Write($"[{tip}]: {DateTime.Now.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture)} - {poruka}\n");

                return true;
            }
            catch
            {
                return false;
            }
        }

        public LoggerServis(string putanja)
        {
            this.putanja = putanja;
        }
    }
}
