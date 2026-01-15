using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modeli
{
    public class VinskiPodrum
    {
        public string Id { get; set; } = string.Empty;
        public string Naziv { get; set; } = string.Empty;
        public int Temperatura { get; set; } = 0;
        public int MaxPaleta { get; set; } = 0;
        public VinskiPodrum() { Id = Guid.NewGuid().ToString(); }
        public VinskiPodrum(string naziv, int temperatura, int maxPaleta)
        {
            Id = Guid.NewGuid().ToString();
            Naziv = naziv;
            Temperatura = temperatura;
            MaxPaleta = maxPaleta;
        }
    }
}
