using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using P_OO_Parking;

// See https://aka.ms/new-console-template for more information
// ETML - 2026 - P_OO
// Createur Hugo Chevrette - CID2A
// Concevoir et réaliser une simulation de parking
// ///////////////////////////////////////////////////

Parking parking = new Parking();
bool quitter = false;

while (!quitter)
{
    Console.Clear();
    parking.Showmenu();
    quitter = parking.ChoixMenu();

    if (!quitter)
    {
        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }
}














