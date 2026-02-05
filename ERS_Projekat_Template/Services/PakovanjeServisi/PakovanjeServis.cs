using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Enumeracije;
using Domain.Servisi;
using Domain.Konstante;

namespace Services.PakovanjeServisi
{
    public class PakovanjeServis : IPakovanjeServis
    {
        IPaletaRepozitorijum paletaRepozitorijum;
        IServisZaProizvodnjuVina servisZaProizvodnju;
        ILoggerServis loggerServis;
        const int BROJ_FLASA = BrojVinaPoPaleti.KOLICINA_VINA_PO_PALETI;

        public PakovanjeServis(IPaletaRepozitorijum paletaRepozitorijum, ILoggerServis loggerServis, IServisZaProizvodnjuVina servisZaProizvodnju)
        {
            this.paletaRepozitorijum=paletaRepozitorijum;
            this.loggerServis=loggerServis;
            this.servisZaProizvodnju = servisZaProizvodnju;
        }

        public Paleta PakovanjeVina()
        {
            try
            {
                List<Vino> vina = servisZaProizvodnju.DobaviVina(BROJ_FLASA);
                IEnumerable<Paleta> palete = paletaRepozitorijum.PregledSvihPaleta();
                
                List<string> IDvina = new List<string>();

                foreach(Vino v in vina)
                {
                    IDvina.Add(v.ID_VINA);
                }
                if (palete.Count() == 0)
                {
                    Paleta paleta = new Paleta();
                    paleta.IDVina = IDvina;
                    Paleta p = paletaRepozitorijum.DodajPaletu(paleta);
                    if (p.SifraPalete != string.Empty)
                    {
                        loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, "Vino uspešno upakovano.");
                        return p;
                    }
                    else
                    {
                        loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspešno pakovanje vina.");
                        return new Paleta();
                    }
                }
                else
                {
                    foreach (Paleta p in palete)
                    {
                        if (p.IDVina.Count == 0)
                        {
                            p.IDVina = IDvina;
                            bool uspeh = paletaRepozitorijum.AzurirajPaletu(p);
                            if (uspeh)
                            {
                                loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, "Vino uspešno upakovano.");
                                return p;
                            }
                            else
                            {
                                loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspešno pakovanje vina.");
                                return new Paleta();
                            }
                        }
                    }
                }
                loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspešno pakovanje vina.");
                return new Paleta();
            }
            catch
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspešno pakovanje vina.");
                return new Paleta();
            }
        }

        public Paleta SlanjePalete(string IDPodruma)
        {
            try
            {
                IEnumerable<Paleta> palete = paletaRepozitorijum.PronadjiPaletuPoStatusu(StatusPalete.UPAKOVANA);
                if(palete.Count() == 0)
                {
                    Paleta paleta = PakovanjeVina();
                    
                    if(paleta.SifraPalete != string.Empty)
                    {
                        paleta.Status = StatusPalete.OTPREMLJENA;
                        paleta.IDPodruma = IDPodruma;
                        Paleta p = paletaRepozitorijum.DodajPaletu(paleta);
                        if (p.SifraPalete != string.Empty)
                        {
                            loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, "Paleta uspešno otpremljena.");
                            return paleta;
                        }
                        else
                        {
                            loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Paleta nije otpremljena.");
                            return new Paleta();
                        }
                    }
                    else
                    {
                        loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Paleta nije otpremljena.");
                        return new Paleta();
                    }
                    
                }
                else
                {
                    Paleta paleta = palete.First();
                    paleta.Status = StatusPalete.OTPREMLJENA;
                    paleta.IDPodruma = IDPodruma;
                    bool uspeh = paletaRepozitorijum.AzurirajPaletu(paleta);
                    if(uspeh)
                    {
                        loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, "Paleta uspešno otpremljena.");
                        return paleta;
                    } 
                    else
                    {
                        loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Paleta nije otpremljena.");
                        return new Paleta();
                    }
                    
                }
            } 
            catch
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Paleta nije otpremljena.");
                return new Paleta();
            }
        }
    }
}
