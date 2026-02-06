using Domain.Enumeracije;
using Domain.Modeli;
using NUnit.Framework;

namespace Tests.Domain
{
    [TestFixture]
    class KorisnikTests
    {
        [TestCase("Marko123", "qwerty123", "Marko Markovic", TipKorisnika.GLAVNI_ENOLOG)]
        [TestCase("Pera", "passwrd456", "Petar Petrovic", TipKorisnika.KELAR_MAJSTOR)]
        [TestCase("Marija", "456321", "Marija Marijanovic", TipKorisnika.KELAR_MAJSTOR)]

        public void KorisnikKonstruktorTest(string korisnickoIme, string lozinka, string imePrezime, TipKorisnika tipKorisnika)
        {
            Korisnik korisnik = new Korisnik(korisnickoIme, lozinka, imePrezime, tipKorisnika);

            Assert.That(korisnik, Is.Not.Null);
            Assert.That(korisnik.KorisnickoIme, Is.EqualTo(korisnickoIme));
            Assert.That(korisnik.Lozinka, Is.EqualTo(lozinka));
            Assert.That(korisnik.ImePrezime, Is.EqualTo(imePrezime));
            Assert.That(korisnik.Uloga, Is.EqualTo(tipKorisnika));
        }
    }
}
