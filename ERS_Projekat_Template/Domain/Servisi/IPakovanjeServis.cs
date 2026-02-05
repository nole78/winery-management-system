using Domain.Enumeracije;
using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IPakovanjeServis
    {
        public Paleta PakovanjeVina();
        public Paleta SlanjePalete(string IDPodruma);
    }
}
