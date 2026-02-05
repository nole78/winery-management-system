using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Services.ServisZaSkladistenje;
using Domain.Repozitorijumi;
using Domain.Modeli;
using Domain.Enumeracije;
using Domain.Konstante;
using Domain.Servisi;
using System.Xml;


namespace Tests.Services.ServisZaSkladistenje
{
    [TestFixture]
    public class ServisZaSkladistenjeTest
    {
        private Mock<IPodrumRepozitorijum> _podrumRepozitorijum;
        private Mock<ILoggerServis> _loggerServis;
        private Mock<IPakovanjeServis> _pakovanjeServis;
        private VinskiPodrumSkladistenjeServis _skladistenjeServis;
        public ServisZaSkladistenjeTest()
        {
            // Inicijalizacija mock objekata
            _podrumRepozitorijum = new Mock<IPodrumRepozitorijum>();
            _loggerServis = new Mock<ILoggerServis>();
            _pakovanjeServis = new Mock<IPakovanjeServis>();

            // Kreiranje stvarnog servisa sa mock zavisnostima
            _skladistenjeServis = new VinskiPodrumSkladistenjeServis(
                _loggerServis.Object,
                _pakovanjeServis.Object,
                _podrumRepozitorijum.Object
            );
        }
        [SetUp]
        public void Setup()
        {
            // Inicijalizacija mock objekata
            _loggerServis = new Mock<ILoggerServis>();
            _pakovanjeServis = new Mock<IPakovanjeServis>();
            _podrumRepozitorijum = new Mock<IPodrumRepozitorijum>();

            // Kreiranje stvarnog servisa sa mock zavisnostima
            _skladistenjeServis = new VinskiPodrumSkladistenjeServis(
                _loggerServis.Object,
                _pakovanjeServis.Object,
                _podrumRepozitorijum.Object
            );
        }

        #region IsporukaPalete Tests

        [Test]
        public void IsporukaPalete_BrojPaletaVeciOdMaksimuma_VracaPraznuListu()
        {
            // Arrange
            int trazeniBrojPaleta = IsporukaBrojPaletaKonstante.VINSKI_PODRUM_MAKS_BROJ_PALETA + 1;
            // Act
            var rezultat = _skladistenjeServis.IsporukaPalete(trazeniBrojPaleta);
            // Assert
            Assert.That(rezultat, Is.Empty);
            _loggerServis.Verify(
                logger => logger.EvidentirajDogadjaj(
                    TipEvidencije.ERROR,
                    It.Is<string>(s => s.Contains($"Maksimalan broj paleta koji se moze dostaviti iz vinskog podruma je {IsporukaBrojPaletaKonstante.VINSKI_PODRUM_MAKS_BROJ_PALETA}"))
                ),
                Times.Once
            );
        }
        [Test]
        public void IsporukaPaleta_BrojPaletaManjiOdMaksimuma_VracaListuPaleta()
        {
            // Arrange
            int trazeniBrojPaleta = IsporukaBrojPaletaKonstante.VINSKI_PODRUM_MAKS_BROJ_PALETA - 1;
            int brojac = 1;
            _pakovanjeServis
                .Setup(p => p.SlanjePalete(It.IsAny<string>()))
                .Returns(() => new Paleta((brojac++).ToString(), "kelar", "123", StatusPalete.OTPREMLJENA));
            _podrumRepozitorijum
                .Setup(p => p.VratiPodrum())
                .Returns(new VinskiPodrum { Id = "123", Naziv = "kelar", Temperatura = 12, MaxPaleta = 100 });

            // Act
            var rezultat = _skladistenjeServis.IsporukaPalete(trazeniBrojPaleta);
            // Assert
            Assert.That(rezultat.Count, Is.EqualTo(trazeniBrojPaleta));

            // Provera da je zabelezena informacija
            _loggerServis.Verify(
                logger => logger.EvidentirajDogadjaj(
                    TipEvidencije.INFO,
                    It.Is<string>(s => s.Contains("Uspešna isporuka paleta iz vinskog podruma."))
                ),
                Times.Once
            );

            // Provera da je metoda SlanjePalete pozvana tacan broj puta
            _pakovanjeServis.Verify(
                x => x.SlanjePalete(It.IsAny<string>()),
                Times.Exactly(trazeniBrojPaleta)
            );

            // Provera da su sve palete jedinstvene
            var uniqueIds = rezultat.Select(p => p.SifraPalete).Distinct().Count();
            Assert.That(uniqueIds, Is.EqualTo(trazeniBrojPaleta));
            
        }
        #endregion
        [TearDown]
        public void TearDown()
        {
            _loggerServis.Reset();
            _podrumRepozitorijum.Reset();
            _pakovanjeServis.Reset();
            _skladistenjeServis = null;
        }
    }
}
