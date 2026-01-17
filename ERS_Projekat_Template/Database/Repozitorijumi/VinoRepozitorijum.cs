using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Enumeracije;
namespace Database.Repozitorijumi
{
    public class VinoRepozitorijum : IVinoRepozitorijum
    {
        IBazaPodataka bazaPodataka;

        public VinoRepozitorijum(IBazaPodataka baza)
        {
            bazaPodataka = baza;
        }

        public Vino PronadjiVinoPoID(string id)
        {
            try
            {
                foreach (var v in bazaPodataka.Tabele.Vina)
                {
                    if (v.ID_VINA == id)
                    {
                        return v;
                    }
                }

                return new Vino();
            }
            catch
            {
                return new Vino();
            }
        }

        public Vino DodajVino(Vino vino)
        {
            try
            {
                bazaPodataka.Tabele.Vina.Add(vino);
                bool uspesno = bazaPodataka.SacuvajPromene();
                if (uspesno)
                {
                    return vino;
                }
                else
                {
                    return new Vino();
                }
            } 
            catch
            {
                return new Vino();
            }
        }

        public IEnumerable<Vino> PronadjiVinoPoTipu(TipVina tip)
        {
            try 
            {
                List<Vino> ret_val = [];
                foreach(var v in bazaPodataka.Tabele.Vina)
                {
                    if(v.Tip == tip)
                    {
                        ret_val.Add(v);
                    }
                }
                return ret_val;
            }
            catch
            {
                return [];
            }
        }

        public bool AzurirajVino(Vino vino)
        {
            try
            {
                for (int i = 0; i < bazaPodataka.Tabele.Vina.Count; i++)
                {
                    if (bazaPodataka.Tabele.Vina[i].ID_VINA == vino.ID_VINA)
                    {
                        bazaPodataka.Tabele.Vina[i] = vino;
                        return bazaPodataka.SacuvajPromene();
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
