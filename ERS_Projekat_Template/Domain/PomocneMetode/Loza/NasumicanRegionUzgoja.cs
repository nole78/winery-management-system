namespace Domain.PomocneMetode.Loza
{
    public static class NasumicanRegionUzgoja
    {
        private static readonly Random random = new();

        private static readonly List<string> moguciRegioni =
        [
            "Negotin", "Vrbas", "Subotica", "Fruksa Gora", "Sremski Karlovci", "Smederevo", "Topola"
        ];

        public static string GenerisiNasumicanRegion()
        {
            int index = random.Next(moguciRegioni.Count);
            return moguciRegioni[index];
        }
    }
}
