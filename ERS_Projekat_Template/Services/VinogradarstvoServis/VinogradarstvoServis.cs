using Domain.Modeli;
using Domain.Servisi;
using Domain.Enumeracije;
using Domain.Repozitorijumi;
using Domain.Konstante.NivoSeceraLoze;
using Domain.PomocneMetode.NasumicneVrednosti;

namespace Services.VinogradarstvoServis
{
    public class VinogradarstvoServis : IVinogradarstvoServis
    {
        ILozaRepozitorijum lozeRepozitorijum;
        ILoggerServis loggerServis;

        public VinogradarstvoServis(ILozaRepozitorijum lozeRepozitorijum, ILoggerServis loggerServis)
        {
            this.lozeRepozitorijum = lozeRepozitorijum;
            this.loggerServis = loggerServis;
        }

        public VinovaLoza PosadiNovuLozu(string naziv)
        {
            try
            {
                if (string.IsNullOrEmpty(naziv))
                {
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Korisnik nije uneo naziv sorte vinove loze prilikom pokusaja sadnje.");
                    return new VinovaLoza();
                }

                VinovaLoza novaLoza = new VinovaLoza();

                Random random = new Random();
                novaLoza.Naziv = NasumicnaLoza.GenerisiNasumicnaLoza();
                novaLoza.NivoSecera = (float)Math.Round(random.NextDouble() * (28.0 - 15.0) + 15.0, 2);
                novaLoza.GodSadnje = DateTime.Now.Year;
                novaLoza.RegionUzgoja = NasumicanRegionUzgoja.GenerisiNasumicanRegion();
                novaLoza.Zrelost = FazaZrelosti.POSADJENA;

                loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, $"Posadjena je nova loza - ID : {novaLoza.Id}, Naziv : {novaLoza.Naziv}, Nivo secera : {novaLoza.NivoSecera}, Region uzgoja : {novaLoza.RegionUzgoja}, Godina sadnje : {novaLoza.GodSadnje}.");

                VinovaLoza dodataLoza = lozeRepozitorijum.DodajLozu(novaLoza);

                return dodataLoza;

            }
            catch
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspesno sadjenje nove loze.");
                return new VinovaLoza();
            }
        }

        public VinovaLoza PromeniNivoSecera(VinovaLoza loza)
        {
            try
            {
                if (loza.NivoSecera < 24.00)
                {
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.WARNING, "Neuspesna regulacija nivoa secera u lozi - secer prosledjene loze je vec u optimalnim granicama.");
                    return new VinovaLoza();
                }

                float prekomerno = loza.NivoSecera - NivoSecera.NIVO_SECERA_GRANICA;

                VinovaLoza novaLoza = PosadiNovuLozu(loza.Naziv);

                novaLoza.NivoSecera = novaLoza.NivoSecera - prekomerno;
                loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, $"Uspesna regulacija nivoa secera u lozi - nivo secera nove loze: {novaLoza.NivoSecera}, nivo secera prosledjene loze: {loza.NivoSecera}.");
                return novaLoza;
            }

            catch
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspesna regulacija nivoa secera u lozi.");
                return new VinovaLoza();
            }
        }

        public IEnumerable<VinovaLoza> OberiLoze(string naziv, int kolicina)
        {

            List<VinovaLoza> loze = lozeRepozitorijum.PregledLozaPoNazivu(naziv).ToList();
            List<VinovaLoza> obraneLoze = [];
            int spremnih = 0;

            try
            {
                foreach (VinovaLoza vl in loze)
                {
                    if (vl.Zrelost == FazaZrelosti.SPREMNA_ZA_BERBU)
                    {
                        spremnih++;
                    }
                }

                if ( spremnih < kolicina)
                {
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.WARNING, "Nema dovoljno loza koje su spremne za berbu.");
                    return Enumerable.Empty<VinovaLoza>();
                }

                int brojObranihLoza = 0;

                for (int i = 0; i < loze.Count(); i++)
                {
                    if (loze[i].Zrelost == FazaZrelosti.SPREMNA_ZA_BERBU)
                    {
                        brojObranihLoza++;
                        loze[i].Zrelost = FazaZrelosti.OBRANA;
                        lozeRepozitorijum.AzurirajLozu(loze[i]);
                        obraneLoze.Add(loze[i]);
                    }

                    if (brojObranihLoza == kolicina) break;
                }

                loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, "Uspesno je obrana zeljena kolicina vinovih loza.");
                return obraneLoze;
            }
            catch
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Branje zeljene kolicine loza nije izvrseno.");
                return Enumerable.Empty<VinovaLoza>();
            }
        }

    }
}

       
