using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modeli
{
    public class StavkaFakture
    {
        public string VinoId { get; set; }
        public string NazivVina { get; set; }
        public double Zapremina { get; set; }
        public int Kolicina { get; set; }
        public decimal JedinicnaCena { get; set; }
    }
}
