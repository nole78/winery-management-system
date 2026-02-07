using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Moq;
using NUnit.Framework;
using Services.LoggerServisi;
using Services.PakovanjeServisi;
using System.Security.Cryptography;

namespace Tests.Services.PakovanjeServisi
{
    [TestFixture]
    public class PakovanjeServisTest
    {
        private Mock<IPaletaRepozitorijum> paletaRepozitorijum;
        private Mock<IServisZaProizvodnjuVina> servisZaProizvodnju;
        private Mock<IPodrumRepozitorijum> podrumRepozitorijum;
        private Mock<ILoggerServis> loggerServis;
        private PakovanjeServis pakovanjeServis;

        public PakovanjeServisTest()
        {
            paletaRepozitorijum = new Mock<IPaletaRepozitorijum>();
            servisZaProizvodnju = new Mock<IServisZaProizvodnjuVina>();
            loggerServis = new Mock<ILoggerServis>();
            podrumRepozitorijum = new Mock<IPodrumRepozitorijum>();

            pakovanjeServis = new PakovanjeServis(paletaRepozitorijum.Object, loggerServis.Object, servisZaProizvodnju.Object, podrumRepozitorijum.Object);
        }

        [SetUp]
        public void Setup()
        {
            paletaRepozitorijum = new Mock<IPaletaRepozitorijum>();
            servisZaProizvodnju = new Mock<IServisZaProizvodnjuVina>();
            loggerServis = new Mock<ILoggerServis>();
            podrumRepozitorijum = new Mock<IPodrumRepozitorijum>();

            pakovanjeServis = new PakovanjeServis(paletaRepozitorijum.Object, loggerServis.Object, servisZaProizvodnju.Object, podrumRepozitorijum.Object);
        }

        [Test]
        public void PakovanjeVinaTest_UspesnoPakovanje()
        {
            List<Vino> _vina = new List<Vino>();
            for(int i = 0;i < 3;i++)
            {
                Vino v = new Vino("123", "Merlot", TipVina.STOLNO, 0.5, "123", new DateTime(1990, 8, 1));
                _vina.Add(v);
            }

            servisZaProizvodnju.Setup(x => x.DobaviVina(1)).Returns(_vina);

            var ret_val = pakovanjeServis.PakovanjeVina();

            Assert.That(ret_val, Is.Not.Null);

            loggerServis.Verify(x => x.EvidentirajDogadjaj(TipEvidencije.INFO, It.Is<string>(msg => msg.Contains("Vino uspešno upakovano."))), Times.Once);
        }

        [Test]
        [TestCase("515")]
        [TestCase("516")]
        [TestCase("517")]
        public void SlanjePaleteTest_UspesnoSlanje_RepoPaletaPrazan(string IDPodruma)
        {
            Paleta paleta = new Paleta("123", "Adresa", IDPodruma, StatusPalete.UPAKOVANA);
            paletaRepozitorijum.Setup(x => x.PronadjiPaletuPoStatusu(StatusPalete.UPAKOVANA)).Returns(new List<Paleta>());

            var ret_val = pakovanjeServis.SlanjePalete(IDPodruma);

            Assert.That(ret_val, Is.Not.Null);
            Assert.That(ret_val, Is.EqualTo(paleta));

            loggerServis.Verify(x => x.EvidentirajDogadjaj(TipEvidencije.INFO, It.Is<string>(msg => msg.Contains("Uspešno slanje palete."))), Times.Once);
        }

        [Test]
        [TestCase("515")]
        [TestCase("516")]
        [TestCase("517")]
        public void SlanjePaleteTest_UspesnoSlanje_RepoPaletaNijePrazan(string IDPodruma)
        {
            Paleta paleta = new Paleta("123", "Adresa", IDPodruma, StatusPalete.UPAKOVANA);
            paletaRepozitorijum.Setup(x => x.DodajPaletu(paleta)).Returns(paleta);

            var ret_val = pakovanjeServis.SlanjePalete(IDPodruma);

            Assert.That(ret_val, Is.Not.Null);
            Assert.That(ret_val, Is.EqualTo(paleta));

            loggerServis.Verify(x => x.EvidentirajDogadjaj(TipEvidencije.INFO, It.Is<string>(msg => msg.Contains("Uspešno slanje palete."))), Times.Once);
        }
    }
}
