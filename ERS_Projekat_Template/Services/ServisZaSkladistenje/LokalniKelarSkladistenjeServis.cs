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
        IPaletaRepozitorijum paletaRepozitorijum;
        const int VREME_PAKOVANJA_SEKUNDE = IsporukaVremeKonstante.LOKALNI_KELAR_ISPORUKA_SEKUNDE;
        const int MAKS_PALETA = IsporukaBrojPaletaKonstante.LOKALNI_KELAR_MAKS_BROJ_PALETA;
        public LokalniKelarSkladistenjeServis(ILoggerServis logger, IPakovanjeServis pakovanje,IPodrumRepozitorijum podrum, IPaletaRepozitorijum paleta)
        {
            loggerServis = logger;
            pakovanjeServis = pakovanje;
            podrumRepozitorijum = podrum;
            paletaRepozitorijum = paleta;
        }

        public List<Paleta> IsporukaPalete(TipVina tipVina, int brojPaleta, double zapreminaFlase, string nazivLoze)
        {
            try
            {
                if(brojPaleta > MAKS_PALETA)
                {
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, $"Maksimalan broj paleta koji se moze dostaviti iz lokalnog kelara je {MAKS_PALETA} - trazeno: {brojPaleta}.");
                    return [];
                }

                List<Paleta> isporucenePalete = [];
                VinskiPodrum kelar = new VinskiPodrum();

                if(podrumRepozitorijum.BrojPodruma() == 0)
                {
                    VinskiPodrum podrum = new VinskiPodrum("Lokalni Kelar", 12, 5);
                    podrumRepozitorijum.DodajPodrum(podrum);
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, $"Napravljen novi podrum '{podrum.Naziv}'.");
                }

                kelar = podrumRepozitorijum.PrviPodrum();

                if(kelar.Naziv == string.Empty)
                {
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspešna isporuka paleta iz lokalnog kelara - ne postoji kelar.");
                    return [];
                }

                while (brojPaleta > 0)
                {
                    Paleta paleta = new Paleta();
                    if (kelar.IDPalete.Count() > brojPaleta)
                    {
                        paleta = paletaRepozitorijum.PronadjiPaletuPoID(kelar.IDPalete[brojPaleta-1]);
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
