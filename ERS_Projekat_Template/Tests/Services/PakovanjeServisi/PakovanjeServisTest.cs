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
        [TestCase("123", "Adresa", "515")]
        [TestCase("124", "Lokalni Kelar", "516")]
        [TestCase("125", "Vinski Podrum", "517")]
        public void PakovanjeVinaTest_UspesnoPakovanje(string sifra,string adresa,string idPodruma)
        {
            List<Vino> _vina = new List<Vino>();
            Paleta paleta = new Paleta(sifra,adresa,idPodruma, StatusPalete.UPAKOVANA);
            for (int i = 0;i < 3;i++)
            {
                Vino v = new Vino("123", "Merlot", TipVina.STOLNO, 0.5, "123", new DateTime(1990, 8, 1));
                _vina.Add(v);
            }
            paletaRepozitorijum.Setup(x => x.DodajPaletu(It.IsAny<Paleta>())).Returns(paleta);
            servisZaProizvodnju.Setup(x => x.DobaviVina(It.IsAny<int>())).Returns(_vina);

            var ret_val = pakovanjeServis.PakovanjeVina();

            Assert.That(ret_val, Is.Not.Null);

            loggerServis.Verify(x => x.EvidentirajDogadjaj(TipEvidencije.INFO, It.Is<string>(msg => msg.Contains("Vino uspešno upakovano."))), Times.Once);
        }

        [Test]
        [TestCase("123", "Adresa", "515")]
        [TestCase("124", "Lokalni Kelar", "516")]
        [TestCase("125", "Vinski Podrum", "517")]
        public void SlanjePaleteTest_UspesnoSlanje_RepoPaletaPrazan(string sifra, string adresa, string idPodruma)
        {
            List<Vino> _vina = new List<Vino>();
            Paleta paleta = new Paleta(sifra, adresa, "", StatusPalete.UPAKOVANA);
            for (int i = 0; i < 3; i++)
            {
                Vino v = new Vino("123", "Merlot", TipVina.STOLNO, 0.5, "123", new DateTime(1990, 8, 1));
                _vina.Add(v);
            }
            podrumRepozitorijum.Setup(x => x.DodajPaletuUPodrum(It.IsAny<string>())).Returns(true);
            paletaRepozitorijum.Setup(x => x.DodajPaletu(It.IsAny<Paleta>())).Returns(paleta);
            servisZaProizvodnju.Setup(x => x.DobaviVina(It.IsAny<int>())).Returns(_vina);
            paletaRepozitorijum.Setup(x => x.PronadjiPaletuPoStatusu(StatusPalete.UPAKOVANA)).Returns(new List<Paleta>() {});
            paletaRepozitorijum.Setup(x => x.AzurirajPaletu(It.IsAny<Paleta>())).Returns(true);

            var ret_val = pakovanjeServis.SlanjePalete(idPodruma);

            Assert.That(ret_val, Is.Not.Null);
            Assert.That(ret_val.SifraPalete,Is.Not.Empty);
        }

        [Test]
        [TestCase("515")]
        [TestCase("516")]
        [TestCase("517")]
        public void SlanjePaleteTest_UspesnoSlanje_RepoPaletaNijePrazan(string IDPodruma)
        {
            Paleta paleta = new Paleta("123", "Adresa", IDPodruma, StatusPalete.UPAKOVANA);
            paletaRepozitorijum.Setup(x => x.PronadjiPaletuPoStatusu(StatusPalete.UPAKOVANA)).Returns(new List<Paleta>() { paleta });
            paletaRepozitorijum.Setup(x => x.AzurirajPaletu(It.IsAny<Paleta>())).Returns(true);

            var ret_val = pakovanjeServis.SlanjePalete(IDPodruma);

            Assert.That(ret_val, Is.Not.Null);
            Assert.That(ret_val.SifraPalete, Is.EqualTo(paleta.SifraPalete));

            loggerServis.Verify(x => x.EvidentirajDogadjaj(TipEvidencije.INFO, It.Is<string>(msg => msg.Contains("Paleta uspešno otpremljena."))), Times.Once);
        }
    }
}
