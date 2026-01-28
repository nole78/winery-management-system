using Database.Repozitorijumi;
using Database.BazaPodataka;
using Domain.BazaPodataka;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Presentation.Authentifikacija;
using Presentation.Meni;
using Services.AutenftikacioniServisi;
using Services.LoggerServisi;
using Services.VinogradarstvoServis;
using Services.ServisiZaProizvodnjuVina;

namespace Loger_Bloger
{
    public class Program
    {
        public static void Main()
        {
            // Baza podataka
            IBazaPodataka bazaPodataka = new XMLBazaPodataka(); // TODO: Initialize the database with appropriate implementation

            // Repozitorijumi
            IKorisniciRepozitorijum korisniciRepozitorijum = new KorisniciRepozitorijum(bazaPodataka);
            IPodrumRepozitorijum podrumRepozitorijum = new PodrumRepozitorijum(bazaPodataka);
            ILozaRepozitorijum lozaRepozitorijum = new LozaRepozitorijum(bazaPodataka);
            IVinoRepozitorijum vinoRepozitorijum = new VinoRepozitorijum(bazaPodataka);

            // Servisi
            IAutentifikacijaServis autentifikacijaServis = new AutentifikacioniServis(); // TODO: Pass necessary dependencies
            // TODO: Add other necessary services
            string putanja = "log.txt";
            ILoggerServis loggerServis = new LoggerServis(putanja);

            // Ako nema nijednog korisnika u sistemu, dodati dva nova
            if (korisniciRepozitorijum.SviKorisnici().Count() == 0)
            {
                // TODO: Add initial users to the system
                korisniciRepozitorijum.DodajKorisnika(new Korisnik("enolog", "enolog123", "Marko Markovic", TipKorisnika.GLAVNI_ENOLOG));
            }

            IVinogradarstvoServis vinogradarstvoServis = new VinogradarstvoServis(lozaRepozitorijum, loggerServis);
            IServisZaProizvodnjuVina servisZaProizvodnjuVina = new ServisZaProizvodnjuVina(vinogradarstvoServis);
            // Prezentacioni sloj
            AutentifikacioniMeni am = new AutentifikacioniMeni(autentifikacijaServis);
            Korisnik prijavljen = new Korisnik();

            while (am.TryLogin(out prijavljen) == false)
            {
                Console.WriteLine("Pogrešno korisničko ime ili lozinka. Pokušajte ponovo.");
            }

            Console.Clear();
            Console.WriteLine($"Uspešno ste prijavljeni kao: {prijavljen.ImePrezime} ({prijavljen.Uloga})");

            OpcijeMeni meni = new OpcijeMeni(); // TODO: Pass necessary dependencies
            meni.PrikaziMeni();
        }
    }
}
