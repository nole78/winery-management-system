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
        const int VREME_PAKOVANJA_SEKUNDE = IsporukaVremeKonstante.VINSKI_PODRUM_ISPORUKA_SEKUNDE;
        const int MAKS_PALETA = IsporukaBrojPaletaKonstante.VINSKI_PODRUM_MAKS_BROJ_PALETA;
        public VinskiPodrumSkladistenjeServis(ILoggerServis logger, IPakovanjeServis pakovanje,IPodrumRepozitorijum podrum)
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
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, $"Maksimalan broj paleta koji se moze dostaviti iz vinskog podruma je {MAKS_PALETA} - trazeno: {brojPaleta}.");
                    return [];
                }

                List<Paleta> isporucenePalete = [];
                VinskiPodrum kelar = podrumRepozitorijum.VratiPodrum();
                if (kelar.Naziv == string.Empty)
                {
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspešna isporuka paleta iz vinskog podruma - ne postoji kelar.");
                    return [];
                }

                while(brojPaleta > 0)
                {
                    Paleta paleta = pakovanjeServis.SlanjePalete(kelar.Id);
                    if(paleta.SifraPalete != string.Empty)
                    {
                        isporucenePalete.Add(paleta);
                    }
                    else
                    {
                        loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspešna isporuka paleta iz vinskog podruma - greška pri pakovanju palete.");
                        return [];
                    }
                    brojPaleta--;
                    Task.Delay(VREME_PAKOVANJA_SEKUNDE).Wait();
                }

                loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, "Uspešna isporuka paleta iz vinskog podruma.");
                return isporucenePalete;
            }
            catch
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspešna isporuka paleta iz vinskog podruma.");
                return [];
            }
        }
    }
}
