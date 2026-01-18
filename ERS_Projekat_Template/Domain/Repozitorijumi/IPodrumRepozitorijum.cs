using Domain.Enumeracije;
using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repozitorijumi
{
    public interface IPodrumRepozitorijum
    {
        public VinskiPodrum DodajPodrum(VinskiPodrum podrum);
        public VinskiPodrum PronadjiVinoPoID(string id);
        public IEnumerable<VinskiPodrum> PregledSvihPodruma();
        public bool AzurirajPodrum(VinskiPodrum podrum);
    }
}
