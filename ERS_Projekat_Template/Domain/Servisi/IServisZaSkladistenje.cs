using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;
using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IServisZaSkladistenje
    {
        List<Paleta> IsporukaPalete(TipVina tipVina, int brojFlasa, double zapreminaFlase, string nazivLoze);
    }
}