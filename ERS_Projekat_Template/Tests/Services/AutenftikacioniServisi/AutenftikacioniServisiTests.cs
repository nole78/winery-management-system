using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Moq;
using NUnit.Framework;
using Services.AutenftikacioniServisi;

namespace Tests.Services.AutenftikacioniServisi
{
    [TestFixture]
    public class AutentifikacioniServisTests
    {
        private Mock<IKorisniciRepozitorijum> _korisniciRepozitorijum;
        private Mock<ILoggerServis> _loggerServis;
        private AutentifikacioniServis _autentifikacioniServis;

        public AutentifikacioniServisTests()
        {
            // Inicijalizacija mock objekata
            _korisniciRepozitorijum = new Mock<IKorisniciRepozitorijum>();
            _loggerServis = new Mock<ILoggerServis>();

            // Kreiranje stvarnog servisa sa mock zavisnostima
            _autentifikacioniServis = new AutentifikacioniServis(
                _korisniciRepozitorijum.Object,
                _loggerServis.Object
            );
        }

        [SetUp]
        public void Setup()
        {
            // Inicijalizacija mock objekata
            _korisniciRepozitorijum = new Mock<IKorisniciRepozitorijum>();
            _loggerServis = new Mock<ILoggerServis>();

            // Kreiranje stvarnog servisa sa mock zavisnostima
            _autentifikacioniServis = new AutentifikacioniServis(
                _korisniciRepozitorijum.Object,
                _loggerServis.Object
            );
        }

        [Test]
        [TestCase("maja", "123")]
        [TestCase("admin", "admin123")]
        public void Prijava_SaIspravnimPodacima_VracaTrueIKorisnika(string korisnickoIme, string lozinka)
        {
            // Arrange - priprema test podataka
            var ocekivaniKorisnik = new Korisnik(korisnickoIme, lozinka, "Maja Majic", TipKorisnika.KELAR_MAJSTOR);

            _korisniciRepozitorijum
                .Setup(x => x.PronadjiKorisnikaPoKorisnickomImenu(korisnickoIme))
                .Returns(ocekivaniKorisnik);

            // Act - izvršavanje metode
            var (uspesnaAutentifikacija, prijavljen) = _autentifikacioniServis.Prijava(korisnickoIme, lozinka);

            // Assert - provera rezultata
            Assert.That(uspesnaAutentifikacija, Is.True);
            Assert.That(prijavljen, Is.Not.Null);
            Assert.That(prijavljen.KorisnickoIme, Is.EqualTo(korisnickoIme));
            Assert.That(prijavljen.Lozinka, Is.EqualTo(lozinka));

            // Provera da je logger pozvan sa ispravnim parametrima
            _loggerServis.Verify(
                x => x.EvidentirajDogadjaj(
                    TipEvidencije.INFO,
                    $"Korisnik '{korisnickoIme}' je uspešno prijavljen."
                ),
                Times.Once
            );
        }

        [Test]
        [TestCase("danijel", "123")]
        [TestCase("jovana", "123")]
        public void Prijava_SaNepostojecimKorisnikom_VracaFalseIPrazanObjekat(string korisnickoIme, string lozinka)
        {
            // Arrange - korisnik ne postoji u bazi
            _korisniciRepozitorijum
                .Setup(x => x.PronadjiKorisnikaPoKorisnickomImenu(korisnickoIme))
                .Returns(new Korisnik()); // prazan objekat

            // Act
            var (uspesnaAutentifikacija, korisnik) = _autentifikacioniServis.Prijava(korisnickoIme, lozinka);

            // Assert
            Assert.That(uspesnaAutentifikacija, Is.False);
            Assert.That(korisnik.KorisnickoIme, Is.Empty);

            // Provera da je zabeleženo upozorenje
            _loggerServis.Verify(
                x => x.EvidentirajDogadjaj(
                    TipEvidencije.WARNING,
                    $"Neuspešna prijava korisnika '{korisnickoIme}'."
                ),
                Times.Once
            );
        }

        [Test]
        [TestCase("maja", "pogresna_lozinka")]
        [TestCase("admin", "wrong123")]
        public void Prijava_SaPogresномLozinkom_VracaFalseIPrazanObjekat(string korisnickoIme, string pogresnaLozinka)
        {
            // Arrange - korisnik postoji ali je lozinka pogrešna
            var postojeciKorisnik = new Korisnik(korisnickoIme, "ispravna_lozinka", "Test User", TipKorisnika.KELAR_MAJSTOR);

            _korisniciRepozitorijum
                .Setup(x => x.PronadjiKorisnikaPoKorisnickomImenu(korisnickoIme))
                .Returns(postojeciKorisnik);

            // Act
            var (uspesnaAutentifikacija, korisnik) = _autentifikacioniServis.Prijava(korisnickoIme, pogresnaLozinka);

            // Assert
            Assert.That(uspesnaAutentifikacija, Is.False);
            Assert.That(korisnik.KorisnickoIme, Is.Empty);

            // Provera logovanja
            _loggerServis.Verify(
                x => x.EvidentirajDogadjaj(
                    TipEvidencije.WARNING,
                    $"Neuspešna prijava korisnika '{korisnickoIme}'."
                ),
                Times.Once
            );
        }

        [Test]
        public void Prijava_SaPraznimKorisnickimImenom_VracaFalse()
        {
            // Arrange
            string praznоKorisnickoIme = "";
            string lozinka = "123";

            _korisniciRepozitorijum
                .Setup(x => x.PronadjiKorisnikaPoKorisnickomImenu(praznоKorisnickoIme))
                .Returns(new Korisnik());

            // Act
            var (uspesnaAutentifikacija, korisnik) = _autentifikacioniServis.Prijava(praznоKorisnickoIme, lozinka);

            // Assert
            Assert.That(uspesnaAutentifikacija, Is.False);
            Assert.That(korisnik.KorisnickoIme, Is.Empty);
        }

        [Test]
        public void Prijava_SaPraznomLozinkom_VracaFalse()
        {
            // Arrange
            string korisnickoIme = "maja";
            string praznaLozinka = "";
            var postojeciKorisnik = new Korisnik(korisnickoIme, "123", "Maja Majic", TipKorisnika.KELAR_MAJSTOR);

            _korisniciRepozitorijum
                .Setup(x => x.PronadjiKorisnikaPoKorisnickomImenu(korisnickoIme))
                .Returns(postojeciKorisnik);

            // Act
            var (uspesnaAutentifikacija, korisnik) = _autentifikacioniServis.Prijava(korisnickoIme, praznaLozinka);

            // Assert
            Assert.That(uspesnaAutentifikacija, Is.False);
        }

        [Test]
        public void Prijava_ProveraVisestrukihNeuspesnihPokusaja()
        {
            // Arrange
            string korisnickoIme = "haker";
            string lozinka = "pokusaj";

            _korisniciRepozitorijum
                .Setup(x => x.PronadjiKorisnikaPoKorisnickomImenu(korisnickoIme))
                .Returns(new Korisnik());

            // Act - simuliranje 3 neuspešna pokušaja
            for (int i = 0; i < 3; i++)
            {
                _autentifikacioniServis.Prijava(korisnickoIme, lozinka);
            }

            // Assert - provera da je logger pozvan 3 puta
            _loggerServis.Verify(
                x => x.EvidentirajDogadjaj(
                    TipEvidencije.WARNING,
                    It.IsAny<string>()
                ),
                Times.Exactly(3)
            );
        }

        [TearDown]
        public void TearDown()
        {
            // Čišćenje resursa nakon svakog testa
            _korisniciRepozitorijum.Reset();
            _loggerServis.Reset();
            _autentifikacioniServis = null;
        }
    }
}