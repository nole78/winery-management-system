using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Enumeracije;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Database.BazaPodataka
{
    public class XMLBazaPodataka : IBazaPodataka
    {
        private readonly string _putanja = Path.Combine("..", "BazaPodataka.xml");

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
            catch
            {
                return false;
            }
        }

        private TabeleBazaPodataka LoadFromXml()
        {
            var tabele = new TabeleBazaPodataka();

            if (!File.Exists(_putanja))
                return tabele;

            var doc = XDocument.Load(_putanja);

            // Users
            tabele.Korisnici = doc.Root?.Element("Korisnici")?
                .Elements("Korisnik")
                .Select(k => new Korisnik
                {
                    Id = long.TryParse(k.Element("ID")?.Value, out var id) ? id : 0,
                    KorisnickoIme = k.Element("Ime")?.Value ?? string.Empty,
                    ImePrezime = k.Element("ImePrezime")?.Value ?? string.Empty,
                    Lozinka = k.Element("Lozinka")?.Value ?? string.Empty,
                    Uloga = Enum.TryParse<TipKorisnika>(k.Element("Uloga")?.Value, out var uloga) ? uloga : TipKorisnika.KELAR_MAJSTOR,
                }).ToList() ?? new List<Korisnik>();

            // Vineyards
            tabele.VinoveLoze = doc.Root?.Element("VinoveLoze")?
                .Elements("VinovaLoza")
                .Select(x => new VinovaLoza
                {
                    Id = x.Element("ID")?.Value ?? string.Empty,
                    Naziv = x.Element("Naziv")?.Value ?? string.Empty,
                    NivoSecera = float.Parse(x.Element("NivoSecera")?.Value ?? "0"),
                    GodSadnje = int.Parse(x.Element("GodinaSadnje")?.Value ?? "0"),
                    RegionUzgoja = x.Element("Region")?.Value ?? string.Empty,
                    Zrelost = Enum.TryParse<FazaZrelosti>(x.Element("Faza")?.Value, out var faza) ? faza : FazaZrelosti.CVETA
                }).ToList() ?? new List<VinovaLoza>();

            // Wines
            tabele.Vina = doc.Root?.Element("Vina")?
                .Elements("Vino")
                .Select(v => new Vino
                {
                    ID_VINA = v.Element("ID_VINA")?.Value ?? string.Empty,
                    Naziv = v.Element("Naziv")?.Value ?? string.Empty,
                    Tip = Enum.TryParse<TipVina>(v.Element("Tip")?.Value, out var tip) ? tip : TipVina.STOLNO,
                    Zapremina = double.Parse(v.Element("Zapremina")?.Value ?? "0"),
                    SifraSerije = v.Element("SifraSerije")?.Value ?? string.Empty,
                    IdLoze = v.Element("IdLoze")?.Value ?? string.Empty,
                    DatumFlasiranja = DateTime.TryParse(v.Element("DatumFlasiranja")?.Value, out var df) ? df : DateTime.Now
                }).ToList() ?? new List<Vino>();

            // Palettes
            tabele.Palete = doc.Root?.Element("Palete")?
                .Elements("Paleta")
                .Select(p => new Paleta
                {
                    SifraPalete = p.Element("SifraPalete")?.Value ?? string.Empty,
                    AdrOdredista = p.Element("AdrOdredista")?.Value ?? string.Empty,
                    IDPodruma = p.Element("IDPodruma")?.Value ?? string.Empty,
                    Status = Enum.TryParse<StatusPalete>(p.Element("Status")?.Value, out var status) ? status : StatusPalete.UPAKOVANA,
                    IDVina = p.Element("Vina")?.Elements("IDVina").Select(x => x.Value).ToList() ?? new List<string>()
                }).ToList() ?? new List<Paleta>();

            // Cellars
            tabele.Podrumi = doc.Root?.Element("Podrumi")?
                .Elements("Podrum")
                .Select(p => new VinskiPodrum
                {
                    Id = p.Element("Id")?.Value ?? string.Empty,
                    Naziv = p.Element("Naziv")?.Value ?? string.Empty,
                    Temperatura = int.TryParse(p.Element("Temperatura")?.Value, out var t) ? t : 0,
                    MaxPaleta = int.TryParse(p.Element("MaxPaleta")?.Value, out var m) ? m : 0,
                }).ToList() ?? new List<VinskiPodrum>();

            // Invoices
            tabele.Fakture = doc.Root?.Element("Fakture")?
                .Elements("Faktura")
                .Select(f => new Faktura
                {
                    Id = f.Element("Id")?.Value ?? string.Empty,
                    DatumKreiranja = DateTime.TryParse(f.Element("DatumKreiranja")?.Value, out var di) ? di : DateTime.Now,
                    TipProdaje = Enum.TryParse<TipProdaje>(f.Element("TipProdaje")?.Value, out var tp) ? tp : TipProdaje.Restoranska,
                    NacinPlacanja = Enum.TryParse<NacinPlacanja>(f.Element("NacinPlacanja")?.Value, out var np) ? np : NacinPlacanja.GotovinskiRacun,
                    UkupanIznos = float.TryParse(f.Element("UkupanIznos")?.Value, out var ui) ? ui : 0,
                    id_vina = f.Element("SpisakVina")?.Elements("VinoId").Select(v => v.Value).ToList() ?? new List<string>()
                }).ToList() ?? new List<Faktura>();

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
                                new XElement("Ime", k.KorisnickoIme),
                                new XElement("ImePrezime", k.ImePrezime),
                                new XElement("Lozinka", k.Lozinka),
                                new XElement("Uloga", k.Uloga.ToString())
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
                                new XElement("Faza", vl.Zrelost.ToString())
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
                                new XElement("DatumFlasiranja", v.DatumFlasiranja.ToString())
                            )
                        )
                    ),

                    new XElement("Palete",
                        tabele.Palete.Select(p =>
                            new XElement("Paleta",
                                new XElement("SifraPalete", p.SifraPalete),
                                new XElement("AdrOdredista", p.AdrOdredista),
                                new XElement("IDPodruma", p.IDPodruma),
                                new XElement("Status", p.Status.ToString()),
                                new XElement("Vina",
                                    p.IDVina.Select(id => new XElement("IDVina", id))
                                )
                            )
                        )
                    ),

                    new XElement("Podrumi",
                        tabele.Podrumi.Select(p =>
                            new XElement("Podrum",
                                new XElement("Id", p.Id),
                                new XElement("Naziv", p.Naziv),
                                new XElement("Temperatura", p.Temperatura),
                                new XElement("MaxPaleta", p.MaxPaleta)
                            )
                        )
                    ),

                    new XElement("Fakture",
                        tabele.Fakture.Select(f =>
                            new XElement("Faktura",
                                new XElement("Id", f.Id),
                                new XElement("DatumKreiranja", f.DatumKreiranja.ToString("o")),
                                new XElement("TipProdaje", f.TipProdaje.ToString()),
                                new XElement("NacinPlacanja", f.NacinPlacanja.ToString()),
                                new XElement("UkupanIznos", f.UkupanIznos),
                                new XElement("SpisakVina",
                                    f.id_vina.Select(id => new XElement("VinoId", id))
                                )
                            )
                        )
                    )
                )
            );

            doc.Save(_putanja);
        }
    }
}