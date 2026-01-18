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
        public StatusPalete Status { get; set; } = StatusPalete.UPAKOVANA;

        public Paleta() { }

        public Paleta(string adrOdredista, long iDPodruma, StatusPalete status)
        {
            AdrOdredista = adrOdredista;
            IDPodruma = iDPodruma;
            Status = status;
        }
    }
}
