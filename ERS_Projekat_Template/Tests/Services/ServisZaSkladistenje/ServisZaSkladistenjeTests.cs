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
using Services.LoggerServisi;


namespace Tests.Services.ServisZaSkladistenje
{
    [TestFixture]
    public class ServisZaSkladistenjeTests
    {
        private Mock<IPodrumRepozitorijum> _podrumRepozitorijum;
        private Mock<ILoggerServis> _loggerServis;
        private Mock<IPakovanjeServis> _pakovanjeServis;
        private VinskiPodrumSkladistenjeServis _skladistenjeServis;
        public ServisZaSkladistenjeTests()
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
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-100)]
        public void IsporukaPaleta_NevalidanBrojPaleta_VracaPraznuListu(int brPaleta)
        {
            // Act
            var rezultat = _skladistenjeServis.IsporukaPalete(brPaleta);
            // Assert
            Assert.That(rezultat, Is.Empty);
            _loggerServis.Verify(
                logger => logger.EvidentirajDogadjaj(
                TipEvidencije.ERROR,
                    It.Is<string>(s => s.Contains("Nevalidan broj paleta trazen."))
                ),
                Times.Once
            );
        }

        [Test]
        [TestCase(100)]
        [TestCase(6)]
        [TestCase(10)]
        public void IsporukaPalete_BrojPaletaVeciOdMaksimuma_VracaPraznuListu(int brPaleta)
        {
            // Act
            var rezultat = _skladistenjeServis.IsporukaPalete(brPaleta);
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
        [TestCase("kelar1","kelar1",StatusPalete.OTPREMLJENA,12,100,3)]
        [TestCase("kelar2", "kelar3", StatusPalete.OTPREMLJENA, 26, 10,5)]
        [TestCase("kelar3", "kelar3", StatusPalete.OTPREMLJENA, 3, 8,1)]
        public void IsporukaPaleta_BrojPaletaManjiOdMaksimuma_VracaListuPaleta(string nazivKelar,string idKelar,StatusPalete status,int temperatura,int maxPaleta,int brPaleta)
        {
            // Arrange
            int brojac = 1;
            _pakovanjeServis
                .Setup(p => p.SlanjePalete(It.IsAny<string>()))
                .Returns(() => new Paleta((brojac++).ToString(),nazivKelar,idKelar,status));
            _podrumRepozitorijum
                .Setup(p => p.VratiPodrum(It.IsAny<int>()))
                .Returns(new VinskiPodrum { Id = idKelar, Naziv = nazivKelar, Temperatura = temperatura, MaxPaleta = maxPaleta });

            // Act
            var rezultat = _skladistenjeServis.IsporukaPalete(brPaleta);
            // Assert
            Assert.That(rezultat.Count, Is.EqualTo(brPaleta));

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
                Times.Exactly(brPaleta)
            );

            // Provera da su sve palete jedinstvene
            var uniqueIds = rezultat.Select(p => p.SifraPalete).Distinct().Count();
            Assert.That(uniqueIds, Is.EqualTo(brPaleta));
            
        }
        #endregion
        [TearDown]
        public void TearDown()
        {
            _loggerServis.Reset();
            _podrumRepozitorijum.Reset();
            _pakovanjeServis.Reset();
            _skladistenjeServis = new VinskiPodrumSkladistenjeServis(
                _loggerServis.Object,
                _pakovanjeServis.Object,
                _podrumRepozitorijum.Object
            );
        }
    }
}
