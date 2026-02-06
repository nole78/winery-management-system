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
    class VinoTest
    {
        [Test]
        [TestCase("211", "Merlot", TipVina.STOLNO, 0.75, "121", "2022/1/31")]
        [TestCase("212", "Pinot Noir", TipVina.KVALITETNO, 1.5, "152", "1990/5/15")]
        [TestCase("217", "Cabernet Sauvignon", TipVina.PREMIJUM, 0.75, "122", "1985/3/9")]

        public void VinoKonstruktorTest(string id, string naziv, TipVina t, double zapremina, string idloze, DateTime df)
        {
            Vino vino = new Vino(id, naziv, t, zapremina, idloze, df);

            Assert.That(vino, Is.Not.Null);
            Assert.That(vino.ID_VINA, Is.EqualTo(id));
            Assert.That(vino.Naziv, Is.EqualTo(naziv));
            Assert.That(vino.Tip, Is.EqualTo(t));
            Assert.That(vino.Zapremina, Is.EqualTo(zapremina));
            Assert.That(vino.IdLoze, Is.EqualTo(idloze));
            Assert.That(vino.DatumFlasiranja, Is.EqualTo(df));
        }

    }
}
