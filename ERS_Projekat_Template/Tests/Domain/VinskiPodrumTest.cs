using Domain.Enumeracije;
using Domain.Modeli;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Domain
{
    [TestFixture]
    class VinskiPodrumTest
    {
        [Test]
        [TestCase("podrum1", 12, 20)]
        [TestCase("podrum2", 9, 18)]
        [TestCase("podrum3", 10, 34)]
        // naziv,temperatura, maxPaleta
        public void VinskiPodrumKonstruktorTest(string naziv, int temperatura, int maxPaleta)
        {
            // Kreiranje potrebnih promenljivih za test
            VinskiPodrum podrum = new(naziv,temperatura,maxPaleta);

            // Provera da li je ocekivani rezultat jednak rezultatu testa
            Assert.That(podrum, Is.Not.Null);
            Assert.That(podrum.Naziv, Is.EqualTo(naziv));
            Assert.That(podrum.Temperatura, Is.EqualTo(temperatura));
            Assert.That(podrum.MaxPaleta, Is.EqualTo(maxPaleta));
        }
    }
}
