using Domain.Modeli;
using Domain.Servisi;
namespace Presentation.Meni
{
    public class OpcijeMeni
    {
        private readonly IServisZaProdaju prodajaServis;
      

        public OpcijeMeni(IServisZaProdaju prodaja)
        {
            prodajaServis = prodaja;
        }

        

        public void PrikaziMeni()
        {
            Console.Clear();

            bool kraj = false;
            while (!kraj)
            {
                Console.WriteLine("\n============================================ MENI ===========================================");
                Console.WriteLine();

                Console.WriteLine("\n1. Prodaja vina\n2. Pregled faktura (GLAVNI ENOLOG)\n3. Odjavite se");
                Console.Write("Opcija: ");
                string? opcija = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(opcija))
                    continue; 


                Console.Write("Unesite redni broj opcije koju birate: ");

                string? izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1":

                       
                        int randomKolicina = new Random().Next(1, 21); 
                        var faktura = prodajaServis.izvrsavanjeProdaje(randomKolicina);
                        Console.WriteLine($"Prodato je {faktura.Kolicina}.");
                        
                        break;
                    case "2":

                        // IMPLEMENTIRATI PREGLED FAKTURA ZA GLAVNOG ENOLOGA
                        break;


                    case "3":
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
