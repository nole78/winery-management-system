using System;
using System.Collections.Generic;
using Domain.Enumeracije;

namespace Domain.Modeli
{
    public class Paleta
    {
        public string SifraPalete { get; set; } = string.Empty; 
        public string AdrOdredista { get; set; } = string.Empty; 
        public long IDPodruma { get; set; } = 0; 
        public List<string> IDVina { get; set; } = new List<string>();
        public StatusPalete Status { get; set; }

        public Paleta() { }

        public Paleta(long sifraPalete, string adrOdredista, long iDPodruma, List<string> iDVina, StatusPalete status)
        {
            SifraPalete = Guid.NewGuid().ToString();
            AdrOdredista = adrOdredista;
            IDPodruma = iDPodruma;
            IDVina = iDVina;
            Status = status;
        }
    }
}
