using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Repozitorijumi;

namespace Services.ServisiZaProizvodnjuVina
{


    public class ServisZaProizvodnjuVina : IServisZaProizvodnjuVina
    {
        private const double LITARA_PO_LOZI = 1.2;

        IVinoRepozitorijum vinoRepozitorijum;


        private readonly IVinogradarstvoServis vinogradarstvoServis;
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
                throw new ArgumentException(nameof(brojFlasa));

            }

            if (zapreminaFlase != 0.75 && zapreminaFlase != 1.5)
            {
                loger.EvidentirajDogadjaj(TipEvidencije.ERROR, "Zapremina flaše mora biti 0.75 ili 1.5 litara.");
                throw new ArgumentException(nameof(zapreminaFlase));

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

        private int IzracunajPotrebnuKolicinuLoza(int brojFlasa, double zapreminaFlase)
        {
            double ukupno = brojFlasa * zapreminaFlase;
            return (int)Math.Ceiling(ukupno / LITARA_PO_LOZI);
        }

        public List<Vino> DobaviVina(TipVina tipVina, int brojFlasa, double zapreminaFlase, string nazivLoze)
        {
            if (brojFlasa <= 0)
            {
                loger.EvidentirajDogadjaj(TipEvidencije.ERROR, "Broj flaša mora biti veći od nule.");
                throw new ArgumentException(nameof(brojFlasa));

            }

            if (zapreminaFlase != 0.75 && zapreminaFlase != 1.5)
            {
                loger.EvidentirajDogadjaj(TipEvidencije.ERROR, "Zapremina flaše mora biti 0.75 ili 1.5 litara.");
                throw new ArgumentException(nameof(zapreminaFlase));

            }



            int potrebnoLoza = IzracunajPotrebnuKolicinuLoza(brojFlasa, zapreminaFlase);

        
            var obraneLoze = vinogradarstvoServis.OberiLoze(nazivLoze, potrebnoLoza).ToList();

            if (obraneLoze.Count < potrebnoLoza)
            {
                loger.EvidentirajDogadjaj(TipEvidencije.ERROR, "Nema dovoljno loza za proizvodnju vina.");
                throw new InvalidOperationException("Nema dovoljno loza za proizvodnju vina.");
            }
            
            return PokreniFermentaciju(tipVina, brojFlasa, zapreminaFlase);
        }

    }


}   
   

