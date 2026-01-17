// See https://aka.ms/new-console-template for more information

//Ovde idu test primeri, nije radilo u glavnom projektu pa sam napravio poseban projekat za testiranje baze podataka
using System;
using Domain.Modeli;
using Domain.Enumeracije;
using Database;

class Program
{
    static void Main(string[] args)
    {
       
        var baza = new XMLBazaPodataka();

       
        var novaLoza = new VinovaLoza(
            naziv: "Chardonnay",
            nivoSecera: 20.5f,
            godSadnje: 2018,
            regionUzgoja: "Novi Sad",
            zrelost: FazaZrelosti.CVETA
        );

        baza.Tabele.VinoveLoze.Add(novaLoza);

        // Save changes
        if (baza.SacuvajPromene())
            Console.WriteLine("Uspesno sacuvano!");
        else
            Console.WriteLine("Neuspesno");

        // Reload and print to confirm
        var baza2 = new XMLBazaPodataka();
        Console.WriteLine("Baza podataka:");
        foreach (var loza in baza2.Tabele.VinoveLoze)
        {
            Console.WriteLine($"- {loza.Naziv} ({loza.NivoSecera} Brix) - {loza.Zrelost}");
        }
    }
}
