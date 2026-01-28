using Domain.Enumeracije;
using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IPakovanjeServis
    {
        public Paleta PakovanjeVina(TipVina tipVina, int brojFlasa, double zapreminaFlase, string nazivLoze);
        public Paleta SlanjePalete(long IDPodruma, TipVina tipVina, int brojFlasa, double zapreminaFlase, string nazivLoze);
    }
}
