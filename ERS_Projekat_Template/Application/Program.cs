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
using Services.PakovanjeServisi;
using Services.ServisZaSkladistenje;
using Services.ServisiZaProdaju;

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
            IPaletaRepozitorijum paletaRepozitorijum = new PaletaRepozitorijum(bazaPodataka);
            IFakturaRepozitorijum fakturaRepozitorijum = new FakturaRepozitorijum(bazaPodataka);
            // Servisi
            string putanja = "log.txt";
            ILoggerServis loggerServis = new LoggerServis(putanja);
            IAutentifikacijaServis autentifikacijaServis = new AutentifikacioniServis(korisniciRepozitorijum,loggerServis);
            IVinogradarstvoServis vinogradarstvoServis = new VinogradarstvoServis(lozaRepozitorijum, loggerServis);
            IServisZaProizvodnjuVina servisZaProizvodnjuVina = new ServisZaProizvodnjuVina(vinoRepozitorijum, vinogradarstvoServis, loggerServis);
            IPakovanjeServis pakovanjeServis = new PakovanjeServis(paletaRepozitorijum, loggerServis, servisZaProizvodnjuVina,podrumRepozitorijum);
            IServisZaSkladistenje servisSkladistenja;
           
            // Ako nema nijednog korisnika u sistemu, dodati dva nova
            if (korisniciRepozitorijum.SviKorisnici().Count() == 0)
            {
                korisniciRepozitorijum.DodajKorisnika(new Korisnik("enolog", "enolog123", "Marko Markovic", TipKorisnika.GLAVNI_ENOLOG));
                korisniciRepozitorijum.DodajKorisnika(new Korisnik("kelar", "kelar123", "Jovana Jovanovic", TipKorisnika.KELAR_MAJSTOR));
            }

            // Prezentacioni sloj
            AutentifikacioniMeni am = new AutentifikacioniMeni(autentifikacijaServis);
            Korisnik prijavljen = new Korisnik();

            while (am.TryLogin(out prijavljen) == false)
            {
                Console.WriteLine("\nPogrešno korisničko ime ili lozinka. Pokušajte ponovo.");
                Console.ReadKey();
                Console.Clear();
            }

            Console.Clear();
            Console.WriteLine($"Uspešno ste prijavljeni kao: {prijavljen.ImePrezime} ({prijavljen.Uloga})");
            Console.WriteLine("Preusmeravanje na meni...");
            Console.ReadKey();

            if (prijavljen.Uloga == TipKorisnika.GLAVNI_ENOLOG)
                servisSkladistenja = new VinskiPodrumSkladistenjeServis(loggerServis, pakovanjeServis, podrumRepozitorijum);
            else
                servisSkladistenja = new LokalniKelarSkladistenjeServis(loggerServis, pakovanjeServis, podrumRepozitorijum);

            IServisZaProdaju servisProdaje = new ServisZaProdaju(vinoRepozitorijum, servisSkladistenja, fakturaRepozitorijum, loggerServis, servisZaProizvodnjuVina);
            OpcijeMeni meni = new OpcijeMeni(fakturaRepozitorijum, vinoRepozitorijum, prijavljen, servisProdaje); 

            meni.PrikaziMeni();
        }
    }
}
