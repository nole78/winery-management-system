using System;
using System.Collections.Generic;
using Domain.Enumeracije;

namespace Domain.Modeli
{
    public class Paleta
    {
        public long SifraPalete { get; set; } = 0; 
        public string AdrOdredista { get; set; } = string.Empty; 
        public long IDPodruma { get; set; } = 0; 
        public List<long> IDVina { get; set; } = new List<long>();
        public StatusPalete Status { get; set; }

        public Paleta() { }

        public Paleta(long sifraPalete, string adrOdredista, long iDPodruma, List<long> iDVina, StatusPalete status)
        {
            SifraPalete = sifraPalete;
            AdrOdredista = adrOdredista;
            IDPodruma = iDPodruma;
            IDVina = iDVina;
            Status = status;
        }
    }
}
