using Domain.Modeli;
using Services.ServisiZaProdaju;
using Domain.Enumeracije;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Presentation.Meni
{
    public class OpcijeMeni
    {

        private readonly IFakturaRepozitorijum fakturaRepozitorijum;
        private readonly IVinoRepozitorijum vinoRepozitorijum;
        private readonly Korisnik korisnik;
        private readonly IServisZaProdaju prodajaServis;
        public OpcijeMeni(IFakturaRepozitorijum faktureRepo, IVinoRepozitorijum vinoRepo, Korisnik kor, IServisZaProdaju prodajaS)
        {
            fakturaRepozitorijum = faktureRepo;
            vinoRepozitorijum = vinoRepo;
            korisnik = kor;
            prodajaServis = prodajaS;
        }


        public void PrikaziMeni()
        {
            Console.Clear();

            bool kraj = false;
            while (!kraj)
            {
                Console.WriteLine("\n============================================ MENI ===========================================");
                Console.WriteLine();


                Console.WriteLine("1. Prodaja vina\n2. Pregled faktura(GLAVNI ENOLOG)\n3. Odjavi se\n"); // kada budu dodate ostale funkcionalnosti prebaciti ovo na kraj

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
                    {
                        if(korisnik.Uloga != TipKorisnika.GLAVNI_ENOLOG)
                        {
                            Console.WriteLine("Nemate dozvolu za ovu opciju!\n");
                            break;
                        }
                        PregledFaktura();
                        break;
                    }
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

        public void PregledFaktura()
        {
            IEnumerable<Faktura> fakture = fakturaRepozitorijum.SveFakture();
            Console.WriteLine("============================================ PREGLED FAKTURA =======================================================\n");
            foreach (Faktura f in fakture)
            {
                Console.WriteLine(f.Header());
                Console.WriteLine(f.ToString());
                Console.WriteLine("PRODATO:\n");
                foreach (string id in f.id_vina)
                {
                    Vino v = vinoRepozitorijum.PronadjiVinoPoID(id);
                    if(id == "")
                    {
                        Console.WriteLine("Greska u trazenju vina");
                        continue;
                    }
                    Console.WriteLine(v.Header());
                    Console.WriteLine(v.ToString());
                }
            }
            Console.WriteLine("====================================================================================================================");
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
