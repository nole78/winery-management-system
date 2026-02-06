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
        IServisZaSkladistenje servisZaSkladistenje;
        IVinoRepozitorijum vinoRepozitorijum;

        public ServisZaProdaju(IVinoRepozitorijum vinoRepozitorijum, IServisZaSkladistenje servisZaSkladistenje,  IFakturaRepozitorijum fakturaRepo, ILoggerServis loggerServis)
        {
            this.fakturaRepo = fakturaRepo;
            this.loggerServis = loggerServis;
            this.servisZaSkladistenje = servisZaSkladistenje;
            this.vinoRepozitorijum = vinoRepozitorijum;    
        }

        public List<Faktura> PregledSvihFaktura()
        {
            return fakturaRepo.SveFakture().ToList();
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
            var odabranaVinaIDs = odabranaVina.Select(v => v.ID_VINA).ToList();
            var faktura = new Faktura(TipProdaje.Restoranska, NacinPlacanja.Gotovina, odabranaVinaIDs, 0, brojFlasa);

            for (int i = 0; i< faktura.Kolicina -1; i++)
            {
                    var vino = vinoRepozitorijum.PronadjiVinoPoID(odabranaVina[i].ID_VINA); 
                    if (vino != null)
                    {
                        faktura.id_vina.Add(vino.ID_VINA);
                        faktura.UkupanIznos += RacunanjeCene.IzracunajCenu(vino);
                    }

            }
           


            loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, $"Izvršena prodaja: {faktura.Kolicina} flaša, Ukupan iznos: {faktura.UkupanIznos} RSD.");



            fakturaRepo.DodajFakturu(faktura);

           

            return faktura;
            
        }
    }


}

