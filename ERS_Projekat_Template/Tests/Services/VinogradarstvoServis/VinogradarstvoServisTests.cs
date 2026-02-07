using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Moq;
using NUnit.Framework;
using Services.VinogradarstvoServis;

namespace Tests.Services.VinogradarstvoServisi
{
    [TestFixture]
    public class VinogradarstvoServisTests
    {
        private Mock<ILozaRepozitorijum> _vinovaLozaRepozitorijum;
        private Mock<ILoggerServis> _loggerServis;
        private VinogradarstvoServis _vinogradarstvoServis;

        public VinogradarstvoServisTests()
        {
            _vinovaLozaRepozitorijum = new Mock<ILozaRepozitorijum>();
            _loggerServis = new Mock<ILoggerServis>();

            _vinogradarstvoServis = new VinogradarstvoServis(_vinovaLozaRepozitorijum.Object, _loggerServis.Object);
        }

        [SetUp]
        public void Setup()
        {
            _vinovaLozaRepozitorijum = new Mock<ILozaRepozitorijum>();
            _loggerServis = new Mock<ILoggerServis>();

            _vinogradarstvoServis = new VinogradarstvoServis(_vinovaLozaRepozitorijum.Object, _loggerServis.Object);
        }


        [Test]
        [TestCase("Chardonnay")]
        [TestCase("Pinot Noir")]

        public void SadiNovuLozu_VracaPosadjenuLozu(string naziv)
        {
            VinovaLoza loza = new VinovaLoza("123",naziv, 15.00f, 2026, "Negotin", FazaZrelosti.POSADJENA);

            _vinovaLozaRepozitorijum.Setup(x => x.DodajLozu(It.IsAny<VinovaLoza>())).Returns(loza);

            VinovaLoza posadjenaLoza = _vinogradarstvoServis.PosadiNovuLozu(naziv);

            Assert.That(posadjenaLoza, Is.Not.Null);
            Assert.That(posadjenaLoza.Naziv, Is.EqualTo(naziv));
            Assert.That(posadjenaLoza.NivoSecera, Is.GreaterThanOrEqualTo(13.00) & Is.LessThanOrEqualTo(28.00));

            //_vinovaLozaRepozitorijum.Verify(x => x.DodajLozu(It.Is<VinovaLoza>(v => v.Zrelost == FazaZrelosti.POSADJENA && v.Naziv == naziv)), Times.Once);

            _loggerServis.Verify(x => x.EvidentirajDogadjaj(TipEvidencije.INFO, It.Is<string>(msg => msg.Contains("Posadjena je nova loza"))), Times.Once);
        }


        [Test]
        [TestCase("Pinot Noir", 27.00f, 2026, "Fruska Gora", FazaZrelosti.SPREMNA_ZA_BERBU)]
        [TestCase("Merlot", 26.00f, 2026, "Negotin", FazaZrelosti.SPREMNA_ZA_BERBU)]

        public void MenjaNivoSecera_VracaLozuSaPromenjenimNivoomSecera(string naziv, float nivoSecera, int godinaSadnje, string regionUzgoja, FazaZrelosti fazaZrelosti)
        {
            VinovaLoza loza = new VinovaLoza("123",naziv, nivoSecera, godinaSadnje, regionUzgoja, fazaZrelosti);

            _vinovaLozaRepozitorijum.Setup(x => x.DodajLozu(It.IsAny<VinovaLoza>())).Returns(loza);

            VinovaLoza korigovanaLoza = _vinogradarstvoServis.PromeniNivoSecera(loza);

            Assert.That(korigovanaLoza, Is.Not.Null);
            Assert.That(korigovanaLoza.NivoSecera, Is.LessThanOrEqualTo(loza.NivoSecera));

            _vinovaLozaRepozitorijum.Verify(x => x.DodajLozu(It.IsAny<VinovaLoza>()), Times.Once);

            _loggerServis.Verify(x => x.EvidentirajDogadjaj(TipEvidencije.INFO, It.Is<string>(msg => msg.Contains("Uspesna regulacija nivoa secera u lozi"))), Times.Once);
        }

        [Test]
        [TestCase("Sauvignon Blanc", 16.00f, 2026, "Fruska Gora", FazaZrelosti.SPREMNA_ZA_BERBU)]
        [TestCase("Chardonnay", 23.99f, 2026, "Negotin", FazaZrelosti.SPREMNA_ZA_BERBU)]

        public void MenjaNivoSecera_VracaGresku_SecerJeVecUGranicama(string naziv, float nivoSecera, int godinaSadnje, string regionUzgoja, FazaZrelosti fazaZrelosti)
        {
            VinovaLoza loza = new VinovaLoza("123", naziv, nivoSecera, godinaSadnje, regionUzgoja, fazaZrelosti);

            _vinovaLozaRepozitorijum.Setup(x => x.DodajLozu(It.IsAny<VinovaLoza>())).Returns(new VinovaLoza());

            VinovaLoza korigovanaLoza = _vinogradarstvoServis.PromeniNivoSecera(loza);

            Assert.That(korigovanaLoza, Is.Not.Null);
            Assert.That(korigovanaLoza.Naziv, Is.Empty);

            _loggerServis.Verify(x => x.EvidentirajDogadjaj(TipEvidencije.WARNING, "Neuspesna regulacija nivoa secera u lozi - secer prosledjene loze je vec u optimalnim granicama."), Times.Once);
        }


        [Test]
        [TestCase("Cabernet Sauvignon", 3)]
        [TestCase("Sauvignon Blanc", 100)]

        public void BranjeGrozdjaOdredjeneSorte_UspesnoBranje_VracaListuObranihLoza(string naziv, int brojLoza)
        {
            List<VinovaLoza> listaLoza = new List<VinovaLoza>();
            for (int i = 0; i < brojLoza; i++)
            {
                listaLoza.Add(new VinovaLoza("123", naziv, 15.00f, 2026, "Negotin", FazaZrelosti.SPREMNA_ZA_BERBU));
            }

            _vinovaLozaRepozitorijum.Setup(x => x.PregledLozaPoNazivu(naziv)).Returns(listaLoza);

            var obraneLoze = _vinogradarstvoServis.OberiLoze(naziv, brojLoza);

            Assert.That(obraneLoze, Is.Not.Null);
            Assert.That(obraneLoze.Count(), Is.EqualTo(brojLoza));
            Assert.That(obraneLoze.All(v => v.Zrelost == FazaZrelosti.OBRANA), Is.True);
            Assert.That(obraneLoze.All(v => v.Naziv == naziv), Is.True);

            _vinovaLozaRepozitorijum.Verify(v => v.AzurirajLozu(It.IsAny<VinovaLoza>()), Times.Exactly(brojLoza));

            _loggerServis.Verify(x => x.EvidentirajDogadjaj(TipEvidencije.INFO, "Uspesno je obrana zeljena kolicina vinovih loza."), Times.Once);

        }

        [Test]
        [TestCase("Pinot Noir", 10)]
        [TestCase("Cabernet Sauvignon", 15)]

        public void BranjeGrozdjaOdredjeneSorte_NeuspesnoBranje_VracaPraznuListu(string naziv, int brojLoza)
        {
            List<VinovaLoza> listaLoza = new List<VinovaLoza>();
            for (int i = 0; i < 5; i++)
            {
                listaLoza.Add(new VinovaLoza("123", naziv, 15.00f, 2026, "Negotin", FazaZrelosti.SPREMNA_ZA_BERBU));
            }

            _vinovaLozaRepozitorijum.Setup(x => x.PregledLozaPoNazivu(naziv)).Returns(listaLoza);

            var obraneLoze = _vinogradarstvoServis.OberiLoze(naziv, brojLoza);

            Assert.That(obraneLoze, Is.Not.Null);
            Assert.That(obraneLoze.Count(), Is.Not.EqualTo(brojLoza));
            Assert.That(obraneLoze.All(v => v.Zrelost == FazaZrelosti.OBRANA), Is.True);

            _loggerServis.Verify(x => x.EvidentirajDogadjaj(TipEvidencije.WARNING, "Nema dovoljno loza koje su spremne za berbu."), Times.Once);

        }


        [TearDown]
        public void TearDown()
        {
            // Čišćenje resursa nakon svakog testa
            _vinovaLozaRepozitorijum.Reset();
            _loggerServis.Reset();
            _vinogradarstvoServis = new VinogradarstvoServis(_vinovaLozaRepozitorijum.Object, _loggerServis.Object);
        }
    }
}
