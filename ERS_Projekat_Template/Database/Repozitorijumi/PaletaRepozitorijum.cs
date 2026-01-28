using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Enumeracije;

namespace Database.Repozitorijumi
{
    public class PaletaRepozitorijum : IPaletaRepozitorijum
    {
        IBazaPodataka bazaPodataka;

        public PaletaRepozitorijum(IBazaPodataka baza)
        {
            bazaPodataka = baza;
        }

        public Paleta DodajPaletu(Paleta paleta)
        {
            try
            {
                paleta.SifraPalete = Guid.NewGuid().ToString();
                bazaPodataka.Tabele.Palete.Add(paleta);
                bool uspesno = bazaPodataka.SacuvajPromene();
                if (uspesno)
                {
                    return paleta;
                }
                else
                {
                    return new Paleta();
                }
            }
            catch
            {
                return new Paleta();
            }
        }

        public Paleta PronadjiPaletuPoID(string id)
        {
            try
            {
                foreach (var p in bazaPodataka.Tabele.Palete)
                {
                    if (p.SifraPalete == id)
                    {
                        return p;
                    }
                }

                return new Paleta();
            }
            catch
            {
                return new Paleta();
            }
        }

        public IEnumerable<Paleta> PronadjiPaletuPoStatusu(StatusPalete status)
        {
            try
            {
                List<Paleta> ret_val = [];
                foreach (var p in bazaPodataka.Tabele.Palete)
                {
                    if (p.Status == status)
                    {
                        ret_val.Add(p);
                    }
                }
                return ret_val;
            }
            catch
            {
                return [];
            }
        }

        public IEnumerable<Paleta> PregledSvihPaleta()
        {
            try
            {
                return bazaPodataka.Tabele.Palete;
            }
            catch
            {
                return [];
            }
        }

        public bool AzurirajPaletu(Paleta paleta)
        {
            try
            {
                for (int i = 0; i < bazaPodataka.Tabele.Palete.Count; i++)
                {
                    if (bazaPodataka.Tabele.Palete[i].SifraPalete == paleta.SifraPalete)
                    {
                        bazaPodataka.Tabele.Palete[i] = paleta;
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
