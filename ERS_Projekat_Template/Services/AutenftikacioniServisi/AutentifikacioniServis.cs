using Domain.Modeli;
using Domain.Servisi;
using Domain.Enumeracije;
using Domain.Repozitorijumi;

namespace Services.AutenftikacioniServisi
{
    public class AutentifikacioniServis : IAutentifikacijaServis
    {
        IKorisniciRepozitorijum korisniciRepo;
        ILoggerServis loggerServis;

        public AutentifikacioniServis(IKorisniciRepozitorijum korisniciRepozitorijum, ILoggerServis logger)
        {
            korisniciRepo = korisniciRepozitorijum;
            loggerServis = logger;
        }
        public (bool, Korisnik) Prijava(string korisnickoIme, string lozinka)
        {
            Korisnik nadjen = korisniciRepo.PronadjiKorisnikaPoKorisnickomImenu(korisnickoIme);

            if(nadjen.KorisnickoIme != string.Empty && nadjen.Lozinka == lozinka)
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, $"Korisnik '{korisnickoIme}' je uspešno prijavljen.");
                return (true, nadjen);
            }
            else
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.WARNING, $"Neuspešna prijava korisnika '{korisnickoIme}'.");
                return(false, new Korisnik());
            }
        }
    }
}
