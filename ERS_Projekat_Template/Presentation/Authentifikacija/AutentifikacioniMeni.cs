using Domain.Modeli;
using Domain.Servisi;

namespace Presentation.Authentifikacija
{
    public class AutentifikacioniMeni
    {
        private readonly IAutentifikacijaServis autentifikacijaServis;

        public AutentifikacioniMeni(IAutentifikacijaServis autentifikacijaServis)
        {
            this.autentifikacijaServis = autentifikacijaServis;
        }

        public bool TryLogin(out Korisnik korisnik)
        {
            Console.WriteLine("\n============================================ PRIJAVA ===========================================");
            Console.WriteLine();

            korisnik = new Korisnik();
            bool uspesnaPrijava = false;
            string? korisnickoIme = "", lozinka = "";

            Console.Write("Korisničko ime: ");
            korisnickoIme = Console.ReadLine() ?? "";

            Console.Write("Lozinka: ");
            lozinka = Console.ReadLine() ?? "";

            (uspesnaPrijava, korisnik) = autentifikacijaServis.Prijava(korisnickoIme.Trim(), lozinka.Trim());

            return uspesnaPrijava;
        }
    }
}
