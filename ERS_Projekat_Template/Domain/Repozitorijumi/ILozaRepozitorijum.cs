using Domain.Enumeracije;
using Domain.Modeli;

namespace Domain.Repozitorijumi
{
    public interface ILozaRepozitorijum
    {
        public VinovaLoza PosadiLozu(VinovaLoza loza);
        public bool PromeniNivoSecera(VinovaLoza loza, int procenat);
        public bool OberiLozu(VinovaLoza loza, int kolicina);
        public IEnumerable<VinovaLoza> PrikaziSveLoze();
        public IEnumerable<VinovaLoza> PrikaziPoNazivu(string naziv);
        public IEnumerable<VinovaLoza> PrikaziPoID(string id);
        public IEnumerable<VinovaLoza> PrikaziPoNivouSecera(float nivo);
        public IEnumerable<VinovaLoza> PrikaziPoGodini(int godina);
        public IEnumerable<VinovaLoza> PrikaziPoRegionu(string region);
        public IEnumerable<VinovaLoza> PrikaziPoZrelosti(FazaZrelosti faza);

    }
}


