using Domain.Modeli;
using Domain.Enumeracije;

namespace Domain.Repozitorijumi
{
    public interface IVinoRepozitorijum
    {
        public Vino DodajVino(Vino vino);
        public Vino PronadjiVinoPoID(string id);
        public IEnumerable<Vino> PronadjiVinoPoTipu(TipVina tip);
        public bool AzurirajVino(Vino vino);
    }
}
