using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.PomocneMetode;

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

        public ServisZaProdaju(IVinoRepozitorijum vinoRepozitorijum, IServisZaSkladistenje servisZaSkladistenje,  IFakturaRepozitorijum fakturaRepo, ILoggerServis loggerServis, IServisZaProizvodnjuVina servisZaProizvodnju)
        {
            this.fakturaRepo = fakturaRepo;
            this.loggerServis = loggerServis;
            this.servisZaProizvodnju = servisZaProizvodnju;
            this.servisZaSkladistenje = servisZaSkladistenje;
            this.vinoRepozitorijum = vinoRepozitorijum;    
        }



        

        public List<Faktura> PregledSvihFaktura()
        {
            return fakturaRepo.SveFakture().ToList();
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
                return new Faktura();
            }

            int brojPaleta = (int)Math.Ceiling((double)brojFlasa / 12);

            
            var palete = servisZaSkladistenje.IsporukaPalete(brojPaleta);
            if (palete.Count == 0)
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Nema dostupnih paleta za isporuku.");
                return new Faktura();
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




            var faktura = Faktura.Kreiraj(TipProdaje.Restoranska, NacinPlacanja.Gotovina, odabranaVina.Count);

            for (int i = 0; i< faktura.Kolicina; i++)
            {
               
                    var vino = vinoRepozitorijum.PronadjiVinoPoID(odabranaVina[i].ID_VINA); 
                    if (vino != null)
                    {
                        faktura.SpisakVina.Add(vino);
                        faktura.UkupanIznos += IzracunajCenu(vino);
                    }

            }
           


            loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, $"Izvršena prodaja: {faktura.Kolicina} flaša, Ukupan iznos: {faktura.UkupanIznos} RSD.");



            fakturaRepo.DodajFakturu(faktura);

           

            return faktura;
        }
    }


}

