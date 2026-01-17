using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Enumeracije;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Database
{
    public class XMLBazaPodataka : IBazaPodataka
    {
        // Putanja do XML fajla baze podataka
        //  private readonly string _putanja = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BazaPodataka.xml");
        //   string projectFolder _putanja= Path.Combine(
        //  Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.FullName,

        private readonly string _putanja = Path.Combine("..", "Database", "BazaPodataka.xml");
  

        public TabeleBazaPodataka Tabele { get; set; }

        public XMLBazaPodataka()
        {
            if (File.Exists(_putanja))
                Tabele = LoadFromXml();
            else
            {
                Tabele = new TabeleBazaPodataka();
                SacuvajPromene();
            }
        }

        public bool SacuvajPromene()
        {
            try
            {
                SaveToXml(Tabele);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Baca se sledeci exception: " + ex.Message);
                return false;
            }
        }

        private TabeleBazaPodataka LoadFromXml()
        {
            var tabele = new TabeleBazaPodataka();

            if (!File.Exists(_putanja))
                return tabele;

            var doc = XDocument.Load(_putanja);


            tabele.Korisnici = doc.Root.Element("Korisnici")?
                .Elements("Korisnik")
                .Select(k => new Korisnik
                {
                    Id = long.TryParse(k.Element("ID")?.Value, out var id) ? id : 0,
                    KorisnickoIme = k.Element("Ime")?.Value
                }).ToList() ?? new List<Korisnik>();


            tabele.VinoveLoze = doc.Root.Element("VinoveLoze")?
                .Elements("VinovaLoza")
                .Select(x => new VinovaLoza
                {
                    Id = x.Element("ID")?.Value,
                    Naziv = x.Element("Naziv")?.Value,
                    NivoSecera = float.Parse(x.Element("NivoSecera")?.Value ?? "0"),
                    GodSadnje = int.Parse(x.Element("GodinaSadnje")?.Value ?? "0"),
                    RegionUzgoja = x.Element("Region")?.Value,
                    Zrelost = Enum.TryParse<FazaZrelosti>(x.Element("Faza")?.Value, out var faza)
                                ? faza
                                : FazaZrelosti.CVETA
                }).ToList() ?? new List<VinovaLoza>();


            tabele.Vina = doc.Root.Element("Vina")?
                .Elements("Vino")
                .Select(v => new Vino
                {
                    ID_VINA = v.Element("ID_VINA")?.Value,
                    Naziv = v.Element("Naziv")?.Value,
                    Tip = Enum.TryParse<TipVina>(v.Element("Tip")?.Value, out var tip) ? tip : TipVina.STOLNO,
                    Zapremina = double.Parse(v.Element("Zapremina")?.Value ?? "0"),
                    SifraSerije = v.Element("SifraSerije")?.Value,
                    IdLoze = v.Element("IdLoze")?.Value,
                    DatumFlasiranja = DateTime.TryParse(v.Element("DatumFlasiranja")?.Value, out var df) ? df : DateTime.Now
                }).ToList() ?? new List<Vino>();


            // tabele.Palete = new List<Paleta>();   // za kasnije kad dodje repozitorijum
            //   tabele.Podrumi = new List<Podrum>();  // za kasnije kad dodje repozitorijum

            return tabele;
        }

        private void SaveToXml(TabeleBazaPodataka tabele)
        {
            var doc = new XDocument(
                new XElement("Database",
                    new XElement("Korisnici",
                        tabele.Korisnici.Select(k =>
                            new XElement("Korisnik",
                                new XElement("ID", k.Id),
                                new XElement("Ime", k.KorisnickoIme)
                            )
                        )
                    ),
                    new XElement("VinoveLoze",
                        tabele.VinoveLoze.Select(vl =>
                            new XElement("VinovaLoza",
                                new XElement("ID", vl.Id),
                                new XElement("Naziv", vl.Naziv),
                                new XElement("NivoSecera", vl.NivoSecera),
                                new XElement("GodinaSadnje", vl.GodSadnje),
                                new XElement("Region", vl.RegionUzgoja),
                                new XElement("Faza", vl.Zrelost)
                            )
                        )
                    ),
                    new XElement("Vina",
                        tabele.Vina.Select(v =>
                            new XElement("Vino",
                                new XElement("ID_VINA", v.ID_VINA),
                                new XElement("Naziv", v.Naziv),
                                new XElement("Tip", v.Tip),
                                new XElement("Zapremina", v.Zapremina),
                                new XElement("SifraSerije", v.SifraSerije),
                                new XElement("IdLoze", v.IdLoze),
                                new XElement("DatumFlasiranja", v.DatumFlasiranja)
                            )
                        )
                    ),
                    new XElement("Palete"),
                    new XElement("Podrumi")
                )
            );

            doc.Save(_putanja);
        }
    }
}