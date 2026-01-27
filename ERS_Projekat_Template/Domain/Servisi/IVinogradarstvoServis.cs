using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IVinogradarstvoServis
    {
        public VinovaLoza PosadiNovuLozu(string naziv);
        public VinovaLoza PromeniNivoSecera(VinovaLoza loza);
        public IEnumerable<VinovaLoza> OberiLoze(string naziv, int kolicina);
    }
}
