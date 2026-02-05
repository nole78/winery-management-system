namespace Presentation.Meni
{
    public class OpcijeMeni
    {

        public void PrikaziMeni()
        {
            Console.Clear();

            bool kraj = false;
            while (!kraj)
            {
                Console.WriteLine("\n============================================ MENI ===========================================");
                Console.WriteLine();

                Console.WriteLine("1. Odjavi se\n"); // kada budu dodate ostale funkcionalnosti prebaciti ovo na kraj
                Console.Write("Unesite redni broj opcije koju birate: ");

                string? izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1":
                    {
                        Odjava();
                        return;                        
                    }
                    default:
                    {
                        Console.WriteLine("\nOdaberite jednu od dostupnih opcija.\n");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    }

                }

            }
        }

        public void Odjava()
        {
            Console.Clear();
            Console.WriteLine("\nUspesno ste se odjavili. Dovidjenja!");
            Console.WriteLine("Povratak na prijavu...");
            Console.ReadKey();
            Console.Clear();
        }
    }

}
