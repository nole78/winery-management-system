using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modeli
{
    public class StavkaFakture
    {
        public string VinoId { get; set; } = string.Empty;
        public string NazivVina { get; set; } = string.Empty;
        public double Zapremina { get; set; } = 0;
        public int Kolicina { get; set; } = 0;
        public float JedinicnaCena { get; set; } = 0;

        public StavkaFakture() { }
    }
}
