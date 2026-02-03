using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;
using Domain.Konstante;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.ServisZaSkladistenje
{
    public class VinskiPodrumSkladistenjeServis : IServisZaSkladistenje
    {
        ILoggerServis loggerServis;
        IPakovanjeServis pakovanjeServis;
        IPodrumRepozitorijum podrumRepozitorijum;
        int VREME_PAKOVANJA_SEKUNDE = IsporukaVremeKonstante.VINSKI_PODRUM_ISPORUKA_SEKUNDE;
        int MAKS_PALETA = IsporukaBrojPaletaKonstante.VINSKI_PODRUM_MAKS_BROJ_PALETA;
        public VinskiPodrumSkladistenjeServis(ILoggerServis logger, IPakovanjeServis pakovanje,IPodrumRepozitorijum podrum)
        {
            loggerServis = logger;
            pakovanjeServis = pakovanje;
            podrumRepozitorijum = podrum;
        }

        public List<Paleta> IsporukaPalete(TipVina tipVina, int brojFlasa, double zapreminaFlase, string nazivLoze)
        {
            try
            {
                List<Paleta> isporucenePalete = [];

                while (brojFlasa > 0)
                {
                    Paleta paleta = pakovanjeServis.PakovanjeVina(tipVina, brojFlasa / MAKS_PALETA, zapreminaFlase, nazivLoze);

                    if (paleta.SifraPalete != string.Empty)
                    {
                        isporucenePalete.Add(paleta);
                        brojFlasa -= brojFlasa / MAKS_PALETA;

                        Task.Delay(VREME_PAKOVANJA_SEKUNDE).Wait();
                    }
                    else
                    {
                        loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspešna isporuka paleta iz lokalnog kelara.");
                        return [];
                    }
                }

                loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, "Uspešna isporuka paleta iz lokalnog kelara.");
                return isporucenePalete;
            }
            catch
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspešna isporuka paleta iz lokalnog kelara.");
                return [];
            }
        }
    }
}
