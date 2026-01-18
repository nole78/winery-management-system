using Domain.Enumeracije;

namespace Domain.Modeli
{
    public class VinovaLoza
    {
        public string Id { get; set; } = string.Empty;
        public string Naziv { get; set; } = string.Empty;
        public float NivoSecera { get; set; } = 0;
        public int GodSadnje { get; set; } = 0;
        public string RegionUzgoja { get; set; } = string.Empty;
        public FazaZrelosti Zrelost { get; set; } = FazaZrelosti.POSADJENA;

        public VinovaLoza(){}

        public VinovaLoza(string naziv, float nivoSecera, int godSadnje, string regionUzgoja, FazaZrelosti zrelost)
        {
            Naziv=naziv;
            NivoSecera=nivoSecera;
            GodSadnje=godSadnje;
            RegionUzgoja=regionUzgoja;
            Zrelost=zrelost;
        }
    }
}
