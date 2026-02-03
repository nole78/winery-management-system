using Domain.Enumeracije;
using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IPakovanjeServis
    {
        public Paleta PakovanjeVina(TipVina tipVina, double zapreminaFlase, string nazivLoze);
        public Paleta SlanjePalete(string IDPodruma, TipVina tipVina, double zapreminaFlase, string nazivLoze);
    }
}
