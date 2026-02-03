using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Repozitorijumi;
 
namespace Database.Repozitorijumi
{
    public class PodrumRepozitorijum : IPodrumRepozitorijum
    {
        IBazaPodataka BazaPodataka;

        public PodrumRepozitorijum(IBazaPodataka bazaPodataka)
        {
            BazaPodataka = bazaPodataka;
        }
        public bool AzurirajPodrum(VinskiPodrum podrum)
        {
            try
            {
                for (int i = 0; i < BazaPodataka.Tabele.Podrumi.Count; i++)
                {
                    if (BazaPodataka.Tabele.Podrumi[i].Id == podrum.Id)
                    {
                        BazaPodataka.Tabele.Podrumi[i] = podrum;
                        return BazaPodataka.SacuvajPromene();
                    }
                }

                return false;
            }
            catch { return false; }
        }

        public int BrojPodruma()
        {
            try
            {
                return BazaPodataka.Tabele.Podrumi.Count;
            }
            catch
            {
                return 0;
            }
        }

        public VinskiPodrum DodajPodrum(VinskiPodrum podrum)
        {
            try
            {
                    podrum.Id = Guid.NewGuid().ToString();
                    BazaPodataka.Tabele.Podrumi.Add(podrum);
                    bool cuvanje = BazaPodataka.SacuvajPromene();

                if(cuvanje)
                    return podrum;
                else
                    return new VinskiPodrum();
            }
            catch 
            { 
                return new VinskiPodrum(); 
            }
        }

        public IEnumerable<VinskiPodrum> PregledSvihPodruma()
        {
            List<VinskiPodrum> lista_podruma = new List<VinskiPodrum>();

            try
            {
                foreach (VinskiPodrum podrum in BazaPodataka.Tabele.Podrumi) 
                    lista_podruma.Add(podrum);
                return lista_podruma;
            }
            catch 
            { 
                return lista_podruma; 
            }
        }

        public VinskiPodrum PronadjiPodrumPoID(string id)
        {
            try
            {
                foreach (VinskiPodrum podrum in BazaPodataka.Tabele.Podrumi)
                {
                    if (podrum.Id == id) 
                        return podrum;
                }

                return new VinskiPodrum();
            }
            catch 
            { 
                return new VinskiPodrum(); 
            }
        }

        public VinskiPodrum PrviPodrum()
        {
            try
            {
                if (BazaPodataka.Tabele.Podrumi.Count > 0)
                    return BazaPodataka.Tabele.Podrumi[0];
                else
                    return new VinskiPodrum();
            }
            catch
            {
                return new VinskiPodrum();
            }
        }
    }
}
