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

        public List<Paleta> IsporukaPalete(TipVina tipVina, int brojFlasa, double zapreminaFlase, string nazivLoze)
        {
            try
            {
                List<Paleta> isporucenePalete = [];
                VinskiPodrum kelar = new VinskiPodrum();

                if(podrumRepozitorijum.BrojPodruma() == 0)
                {
                    VinskiPodrum podrum = new VinskiPodrum("Lokalni Kelar", 12, 10);
                    podrumRepozitorijum.DodajPodrum(podrum);
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, $"Napravljen novi podrum '{podrum.Naziv}'.");
                }

                if(kelar.Naziv == string.Empty)
                {
                    return [];
                }

                while (brojFlasa > 0)
                {
                    Paleta paleta = pakovanjeServis.SlanjePalete(kelar.Id,tipVina, brojFlasa / MAKS_PALETA, zapreminaFlase,nazivLoze); // TODO : Izmeniti ID podruma

                    if (paleta.SifraPalete != string.Empty)
                    {
                        isporucenePalete.Add(paleta);
                        brojFlasa -= brojFlasa/MAKS_PALETA;

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
