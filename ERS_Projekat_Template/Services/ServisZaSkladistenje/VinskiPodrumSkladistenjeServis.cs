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
        IPaletaRepozitorijum paletaRepozitorijum;
        const int VREME_PAKOVANJA_SEKUNDE = IsporukaVremeKonstante.VINSKI_PODRUM_ISPORUKA_SEKUNDE;
        const int MAKS_PALETA = IsporukaBrojPaletaKonstante.VINSKI_PODRUM_MAKS_BROJ_PALETA;
        public VinskiPodrumSkladistenjeServis(ILoggerServis logger, IPakovanjeServis pakovanje,IPodrumRepozitorijum podrum, IPaletaRepozitorijum paletaRepozitorijum)
        {
            loggerServis = logger;
            pakovanjeServis = pakovanje;
            podrumRepozitorijum = podrum;
            this.paletaRepozitorijum = paletaRepozitorijum;
        }

        public List<Paleta> IsporukaPalete(TipVina tipVina, int brojPaleta, double zapreminaFlase, string nazivLoze)
        {
            try
            {
                if (brojPaleta > MAKS_PALETA)
                {
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, $"Maksimalan broj paleta koji se moze dostaviti iz vinskog podruma je {MAKS_PALETA} - trazeno: {brojPaleta}.");
                    return [];
                }

                List<Paleta> isporucenePalete = [];
                VinskiPodrum kelar = new VinskiPodrum();

                if (podrumRepozitorijum.BrojPodruma() == 0)
                {
                    VinskiPodrum podrum = new VinskiPodrum("Vinski Podrum", 12, 10);
                    podrumRepozitorijum.DodajPodrum(podrum);
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, $"Napravljen novi podrum '{podrum.Naziv}'.");
                }

                kelar = podrumRepozitorijum.PrviPodrum();

                if (kelar.Naziv == string.Empty)
                {
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspešna isporuka paleta iz vinskog podruma - ne postoji kelar.");
                    return [];
                }

                while (brojPaleta > 0)
                {
                    Paleta paleta = new Paleta();
                    if (kelar.IDPalete.Count() > brojPaleta)
                    {
                        paleta = paletaRepozitorijum.PronadjiPaletuPoID(kelar.IDPalete[brojPaleta - 1]);
                    }
                    else
                        paleta = pakovanjeServis.SlanjePalete(kelar.Id, tipVina, zapreminaFlase, nazivLoze);

                    if (paleta.SifraPalete != string.Empty)
                    {
                        isporucenePalete.Add(paleta);
                        Task.Delay(VREME_PAKOVANJA_SEKUNDE).Wait();
                    }
                    else
                    {
                        loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspešna isporuka paleta iz vinskog podruma.");
                        return [];
                    }
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
