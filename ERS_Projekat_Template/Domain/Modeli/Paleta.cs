using System;
using System.Collections.Generic;
using Domain.Enumeracije;

namespace Domain.Modeli
{
    public class Paleta
    {
        public string SifraPalete { get; set; } = string.Empty; 
        public string AdrOdredista { get; set; } = string.Empty; 
        public string IDPodruma { get; set; } = string.Empty; 
        public List<string> IDVina { get; set; } = new List<string>();
        public StatusPalete Status { get; set; } = StatusPalete.UPAKOVANA;

        public Paleta() { }

        public Paleta(string adrOdredista, string iDPodruma, StatusPalete status)
        {
            AdrOdredista = adrOdredista;
            IDPodruma = iDPodruma;
            Status = status;
        }
    }
}
