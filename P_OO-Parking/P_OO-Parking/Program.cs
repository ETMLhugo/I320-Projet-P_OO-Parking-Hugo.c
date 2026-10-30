using P_OO_Parking;

// Parking utilisé par le programme.
Parking parking = new Parking();
// Indique si l’utilisateur souhaite quitter.
bool quitter = false;

while (!quitter)
{
    Console.Clear();
    parking.AfficherMenu();
    quitter = parking.ChoixMenu();

    if (!quitter)
    {
        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }
}
