using Domain.Enumeracije;
using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Servisi
{
    public interface IServisZaProdaju
    {
        public Faktura izvrsavanjeProdaje(int brojFlasa);

        public List<Faktura> PregledSvihFaktura();

    }
}