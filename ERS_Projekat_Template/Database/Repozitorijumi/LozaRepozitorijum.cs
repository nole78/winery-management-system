using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Repozitorijumi;

namespace Database.Repozitorijumi
{
    public class LozaRepozitorijum : ILozaRepozitorijum
    {
        IBazaPodataka BazaPodataka;

        public LozaRepozitorijum(IBazaPodataka bazaPodataka)
        {
            BazaPodataka = bazaPodataka;
        }

        public VinovaLoza DodajLozu(VinovaLoza loza)
        {
            try
            {
                VinovaLoza postoji = PronadjiLozuPoID(loza.Id);

                if (postoji.Id == string.Empty)
                {
                    loza.Id = Guid.NewGuid().ToString();
                    BazaPodataka.Tabele.VinoveLoze.Add(loza);
                    BazaPodataka.SacuvajPromene();

                    return loza;
                }

                return new VinovaLoza();
            }
            catch { return new VinovaLoza(); }

        }

        public bool AzurirajLozu(VinovaLoza loza)
        {
            try
            {
                for (int i = 0; i < BazaPodataka.Tabele.VinoveLoze.Count; i++)
                {
                    if (BazaPodataka.Tabele.VinoveLoze[i].Id == loza.Id)
                    {
                        BazaPodataka.Tabele.VinoveLoze[i] = loza;
                        return BazaPodataka.SacuvajPromene();
                    }
                }

                return false;
            }
            catch { return false; }
            
        }

        public VinovaLoza PronadjiLozuPoID(string id)
        {
            try
            {
                foreach (VinovaLoza vl in BazaPodataka.Tabele.VinoveLoze)
                {
                    if (vl.Id == id) return vl;
                }

                return new VinovaLoza();
            }
            catch { return new VinovaLoza(); }
        }

        public IEnumerable<VinovaLoza> PregledLozaPoSeceru(float nivoSecera)
        {
            List<VinovaLoza> lista = new List<VinovaLoza>();

            try
            {
                foreach (VinovaLoza vl in BazaPodataka.Tabele.VinoveLoze)
                {
                    if (vl.NivoSecera == nivoSecera) lista.Add(vl);
                }

                return lista;
            }
            catch { return lista; }
        }

        public IEnumerable<VinovaLoza> PregledSvihLoza()
        {
            List<VinovaLoza> lista = new List<VinovaLoza>();

            try
            {
                foreach (VinovaLoza vl in BazaPodataka.Tabele.VinoveLoze) lista.Add(vl);
                return lista;
            }
            catch { return lista; }
        }

    } 
}
