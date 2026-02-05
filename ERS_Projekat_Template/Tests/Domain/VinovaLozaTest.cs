using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;
using Domain.Modeli;
using NUnit.Framework;

namespace Tests.Domain
{
    [TestFixture]
    class VinovaLozaTest
    {
        [Test]
        [TestCase("123", "Merlot", 17, 2025, "Smederevo", FazaZrelosti.POSADJENA)]
        [TestCase("1245", "Cabernet Sauvignon", 25, 2026, "Negotin", FazaZrelosti.SPREMNA_ZA_BERBU)]
        [TestCase("1256", "Chardonnay", 22, 2026, "Fruska Gora", FazaZrelosti.CVETA)]

        public void VinovaLozaKonstruktorTest(string id, string naziv, float nivoSecera, int godinaSadnje, string regionUzgoja, FazaZrelosti zrelost)
        {
            VinovaLoza loza = new VinovaLoza(id, naziv, nivoSecera, godinaSadnje, regionUzgoja, zrelost);

            Assert.That(loza, Is.Not.Null);
            Assert.That(loza.Id, Is.EqualTo(id));
            Assert.That(loza.Naziv, Is.EqualTo(naziv));
            Assert.That(loza.NivoSecera, Is.EqualTo(nivoSecera));
            Assert.That(loza.GodSadnje, Is.EqualTo(godinaSadnje));
            Assert.That(loza.RegionUzgoja, Is.EqualTo(regionUzgoja));
            Assert.That(loza.Zrelost, Is.EqualTo(zrelost));
        }
    }
}
