using Domain.Modeli;
using Domain.Enumeracije;

namespace Domain.Repozitorijumi
{
    public interface IPaletaRepozitorijum
    {
        public Paleta DodajPaletu(Paleta paleta);
        public Paleta PronadjiPaletuPoID(string id);
        public IEnumerable<Paleta> PronadjiPaletuPoStatusu(StatusPalete status);
        public IEnumerable<Paleta> PregledSvihPaleta();
        public bool AzurirajPaletu(Paleta paleta);
    }
}
