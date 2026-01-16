using Domain.Modeli;

namespace Domain.Repozitorijumi
{
    public interface ILozaRepozitorijum
    {
        public VinovaLoza DodajLozu(VinovaLoza loza);
        public bool AzurirajLozu(VinovaLoza loza);
        public VinovaLoza PronadjiLozuPoID(string id);
        public IEnumerable<VinovaLoza> PregledLozaPoSeceru(float nivoSecera);
        public IEnumerable<VinovaLoza> PregledSvihLoza();

    }
}
