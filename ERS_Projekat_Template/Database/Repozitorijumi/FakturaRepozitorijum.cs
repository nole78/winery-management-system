using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Repozitorijumi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repozitorijumi
{
    public class FakturaRepozitorijum : IFakturaRepozitorijum
    {
        IBazaPodataka bazaPodataka;
        public FakturaRepozitorijum(IBazaPodataka baza)
        {
            bazaPodataka = baza;
        }
        public Faktura DodajFakturu(Faktura faktura)
        {
            try
            {




                if (!PronadjiFakturuPoIDu(faktura.Id))
                {




                    bazaPodataka.Tabele.Fakture.Add(faktura);


                    bazaPodataka.SacuvajPromene();

                    return faktura;
                }


                return new Faktura();
            }
            catch
            {

                return new Faktura();
            }
        }



        public Boolean PronadjiFakturuPoIDu(string Id)
        {
            try
            {

                foreach (Faktura faktura in bazaPodataka.Tabele.Fakture)
                {
                    if (faktura.Id == Id)
                        return true;
                }


                return false;
            }
            catch
            {

                return false;
            }
        }

        public IEnumerable<Faktura> SveFakture()
        {
            try
            {
                return bazaPodataka.Tabele.Fakture;
            }
            catch
            {
                return [];
            }



        }
    }
}
