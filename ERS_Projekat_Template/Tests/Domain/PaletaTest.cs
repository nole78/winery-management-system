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
    class PaletaTest
    {
        [TestCase("312", "podrum1", "412", StatusPalete.UPAKOVANA)]
        [TestCase("313", "podrum2", "414", StatusPalete.OTPREMLJENA)]
        [TestCase("315", "podrum3", "417", StatusPalete.UPAKOVANA)]

        public void PaletaKonstruktorTest(string sifra, string adrOdredista, string iDPodruma, StatusPalete status)
        {
            Paleta paleta = new Paleta(sifra, adrOdredista, iDPodruma, status);

            Assert.That(paleta, Is.Not.Null);
            Assert.That(paleta.SifraPalete, Is.EqualTo(sifra));
            Assert.That(paleta.AdrOdredista, Is.EqualTo(adrOdredista));
            Assert.That(paleta.IDPodruma, Is.EqualTo(iDPodruma));
            Assert.That(paleta.Status, Is.EqualTo(status));
        }
    }
}
