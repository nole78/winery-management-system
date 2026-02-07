using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Repozitorijumi;
using Domain.Konstante;
using Domain.PomocneMetode;
using Domain.PomocneMetode.NasumicneVrednosti;

namespace Services.ServisiZaProizvodnjuVina
{


    public class ServisZaProizvodnjuVina : IServisZaProizvodnjuVina
    {
        IVinoRepozitorijum vinoRepozitorijum;
        IVinogradarstvoServis vinogradarstvoServis;
        ILoggerServis loger;

        public ServisZaProizvodnjuVina(IVinoRepozitorijum vinoRepozitorijum, IVinogradarstvoServis vinogradarstvoServis, ILoggerServis loger)
        {
            this.vinoRepozitorijum = vinoRepozitorijum;
            this.vinogradarstvoServis = vinogradarstvoServis;
            this.loger = loger;
        }

        public List<Vino> PokreniFermentaciju(TipVina tipVina, int brojFlasa, double zapreminaFlase)
        {
            if (brojFlasa <= 0)
            {
                loger.EvidentirajDogadjaj(TipEvidencije.ERROR, "Broj flaša mora biti veći od nule.");
                return [];

            }

            if (zapreminaFlase != 0.75 && zapreminaFlase != 1.5)
            {
                loger.EvidentirajDogadjaj(TipEvidencije.ERROR, "Zapremina flaše mora biti 0.75 ili 1.5 litara.");
                return [];

            }

           

            var vina = new List<Vino>();

            for (int i = 0; i < brojFlasa; i++)
            {
                vina.Add(new Vino
                {
                    Tip = tipVina,
                    Zapremina = zapreminaFlase,
                    DatumFlasiranja = DateTime.Now
                });
                loger.EvidentirajDogadjaj(TipEvidencije.INFO, $"Proizvedeno vino tipa {tipVina} sa zapreminom {zapreminaFlase}L.");
                vinoRepozitorijum.DodajVino(vina.Last());   
            }

            return vina;
        }
        public List<Vino> DobaviVina(int brojFlasa)
        {
            if (brojFlasa <= 0)
            {
                loger.EvidentirajDogadjaj(TipEvidencije.ERROR, "Broj flaša mora biti veći od nule.");
                return [];

            }

            double zapreminaFlase = NasumicnaZapreminaFlase.GenerisiNasumicnaZapreminaFlase();
            string nazivLoze = NasumicnaLoza.GenerisiNasumicnaLoza();
            TipVina tipVina = NasumicanTipVina.GenerisiNasumicanTipVina();


            int potrebnoLoza = PotrebnaKolicinaLoza.IzracunajPotrebnuKolicinuLoza(brojFlasa, zapreminaFlase);

        
            var obraneLoze = vinogradarstvoServis.OberiLoze(nazivLoze, potrebnoLoza).ToList();

            while (obraneLoze.Count < potrebnoLoza)
            {
                VinovaLoza loza = vinogradarstvoServis.PosadiNovuLozu(NasumicnaLoza.GenerisiNasumicnaLoza());
                if(loza.Naziv == string.Empty)
                {
                    loger.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspesno sadjenje nove loze.");
                    return [];
                }

                if (loza.NivoSecera > 24)
                    loza = vinogradarstvoServis.PromeniNivoSecera(loza);
                
                if(loza.Naziv == string.Empty)
                {
                    loger.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspesna regulacija nivoa secera u lozi.");
                    return [];
                }
                obraneLoze.Add(loza);
            }
            
            return PokreniFermentaciju(tipVina, brojFlasa, zapreminaFlase);
        }

    }


}   
   

