using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Konstante;
using Domain.Modeli;
using Domain.Servisi;
using Domain.Enumeracije;
using Domain.Repozitorijumi;
using System.ComponentModel.Design;

namespace Services.ServisZaSkladistenje
{
    public class LokalniKelarSkladistenjeServis : IServisZaSkladistenje
    {
        ILoggerServis loggerServis;
        IPakovanjeServis pakovanjeServis;
        IPodrumRepozitorijum podrumRepozitorijum;
        const int VREME_PAKOVANJA_SEKUNDE = IsporukaVremeKonstante.LOKALNI_KELAR_ISPORUKA_SEKUNDE;
        const int MAKS_PALETA = IsporukaBrojPaletaKonstante.LOKALNI_KELAR_MAKS_BROJ_PALETA;
        public LokalniKelarSkladistenjeServis(ILoggerServis logger, IPakovanjeServis pakovanje,IPodrumRepozitorijum podrum)
        {
            loggerServis = logger;
            pakovanjeServis = pakovanje;
            podrumRepozitorijum = podrum;
        }
        public List<Paleta> IsporukaPalete(int brojPaleta)
        {
            try
            {
                if (brojPaleta > MAKS_PALETA)
                {
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, $"Maksimalan broj paleta koji se moze dostaviti iz lokalnog kelara je {MAKS_PALETA} - trazeno: {brojPaleta}.");
                    return [];
                }
                else if (brojPaleta <= 0)
                {
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Nevalidan broj paleta trazen.");
                    return [];
                }

                List<Paleta> isporucenePalete = [];
                VinskiPodrum kelar = podrumRepozitorijum.VratiPodrum(brojPaleta);
                if (kelar.Naziv == string.Empty)
                {
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspešna isporuka paleta iz lokalnog kelara - ne postoji kelar.");
                    return [];
                }

                while (brojPaleta > 0)
                {
                    Paleta paleta = pakovanjeServis.SlanjePalete(kelar.Id);
                    if (paleta.SifraPalete != string.Empty)
                    {
                        isporucenePalete.Add(paleta);
                    }
                    else
                    {
                        loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspešna isporuka paleta iz lokalnog kelara - greška pri pakovanju palete.");
                        return [];
                    }
                    brojPaleta--;
                    Task.Delay(VREME_PAKOVANJA_SEKUNDE).Wait();
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
