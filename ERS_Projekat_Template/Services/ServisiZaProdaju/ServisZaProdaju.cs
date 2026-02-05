using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ServisiZaProdaju
{
    public class ServisZaProdaju : IServisZaProdaju
    {

        IFakturaRepozitorijum fakturaRepo;
        ILoggerServis loggerServis;
        IServisZaProizvodnjuVina servisZaProizvodnju;
        IVinoRepozitorijum vinoRepo;
        IServisZaSkladistenje servisZaSkladistenje;
        IVinoRepozitorijum vinoRepozitorijum;


        private readonly Random random = new Random();
        public ServisZaProdaju(IVinoRepozitorijum vinoRepozitorijunm, IServisZaSkladistenje servisZaSkladistenje, IVinoRepozitorijum vinoRepo, IFakturaRepozitorijum fakturaRepo, ILoggerServis loggerServis, IServisZaProizvodnjuVina servisZaProizvodnju)
        {
            this.fakturaRepo = fakturaRepo;
            this.loggerServis = loggerServis;
            this.servisZaProizvodnju = servisZaProizvodnju;
            this.vinoRepo = vinoRepo;
            this.servisZaSkladistenje = servisZaSkladistenje;
            this.vinoRepozitorijum = vinoRepozitorijunm;    
        }



        

        public List<Faktura> PregledSvihFaktura()
        {
            return fakturaRepo.SveFakture().ToList();
        }


        public TipVina NasumicanTipVina()
        {
            var values = Enum.GetValues(typeof(TipVina));
            return (TipVina)values.GetValue(random.Next(values.Length));
        }

       

        public double NasumicnaZapremina()
        {
            return random.Next(2) == 0 ? 0.75 : 1.5;
        }


        public string NasumicnaLoza()
        {
            var loze = new List<string>
            {
                "Merlot",
                "Cabernet Sauvignon",
                "Chardonnay",
                "Pinot Noir",
                "Sauvignon Blanc"
            };

            return loze[random.Next(loze.Count)];
        }

        public float IzracunajCenu(Vino vino)
        {
            return vino.Tip switch
            {
                TipVina.STOLNO => 800,
                TipVina.KVALITETNO => 1400,
                TipVina.PREMIJUM => 2500,
                _ => 1000
            };
        }



        public Faktura izvrsavanjeProdaje(int brojFlasa)
        {
            if (brojFlasa <= 0)
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Broj flaša mora biti veći od nule.");
                throw new ArgumentException(nameof(brojFlasa));
            }

          
            var tipVina = NasumicanTipVina();
            var nazivLoze = NasumicnaLoza();
            var zapremina = NasumicnaZapremina();

            loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO,
                $"Pokrenuta prodaja: {brojFlasa} flaša, tip {tipVina}, loza {nazivLoze}, zapremina {zapremina}L.");

          
            int brojPaleta = (int)Math.Ceiling((double)brojFlasa / 12);

            
            var palete = servisZaSkladistenje.IsporukaPalete(tipVina, brojPaleta, zapremina, nazivLoze);
            if (palete.Count == 0)
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Nema dostupnih paleta za isporuku.");
                throw new InvalidOperationException("Nema dostupnih paleta za isporuku.");
            }

         
            var vinaZaProdaju = new List<Vino>();
            

            foreach (var paleta in palete)
            {
                foreach (var idVina in paleta.IDVina)
                {
                    var vino = vinoRepozitorijum.PronadjiVinoPoID(idVina);
                    if (vino != null)
                        vinaZaProdaju.Add(vino);
                }
            }


            var odabranaVina = vinaZaProdaju.Take(brojFlasa).ToList();

            
            var stavke = odabranaVina
                .GroupBy(v => v.ID_VINA)
                .Select(g => new StavkaFakture
                {
                    NazivVina = g.First().Naziv,
                    JedinicnaCena = IzracunajCenu(g.First()), // now float
                    Kolicina = g.Count()
                })
                .ToList();

            // 7️⃣ Create Faktura (TipProdaje and NacinPlacanja can be defaulted or random)
            var faktura = Faktura.Kreiraj(
                TipProdaje.Restoranska,
                NacinPlacanja.Gotovina,
                stavke
            );

            // 8️⃣ Save Faktura
            fakturaRepo.DodajFakturu(faktura);

            loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, $"Kreirana faktura {faktura.Id} sa {stavke.Count} stavki.");

            return faktura;
        }
    }


}

