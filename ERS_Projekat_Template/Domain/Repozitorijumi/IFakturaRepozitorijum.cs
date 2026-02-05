using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repozitorijumi
{
    public interface IFakturaRepozitorijum
    {
        public Faktura DodajFakturu(Faktura faktura);
        public Boolean PronadjiFakturuPoIDu(Guid Id);
        public IEnumerable<Faktura> SveFakture();
    }
}
