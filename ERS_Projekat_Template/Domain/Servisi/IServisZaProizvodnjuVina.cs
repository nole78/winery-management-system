using Domain.Enumeracije;
using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Servisi
{
    public interface IServisZaProizvodnjuVina
    {

  
        List<Vino> PokreniFermentaciju(TipVina tipVina, int brojFlasa, double zapreminaFlase);

        List<Vino> DobaviVina(TipVina tipVina,int brojFlasa,double zapreminaFlase, string nazivLoze);
    }
}
