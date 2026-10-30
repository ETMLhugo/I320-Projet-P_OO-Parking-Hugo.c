using P_OO_Parking;
using System.Collections;
using System.Reflection;

System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

// Nombre de tests réussis.
int reussis = 0;
// Nombre de tests échoués.
int echoues = 0;
// Sortie de la console utilisée pour les résultats.
TextWriter sortie = Console.Out;
// Liste des résultats des tests.
List<string[]> resultats = new List<string[]>();

// Vérifie que le résultat du test est correct.
// condition : Résultat à vérifier.
void Verifier(bool condition)
{
    if (!condition)
    {
        throw new Exception("Résultat différent du résultat attendu.");
    }
}

// Récupère le texte affiché pendant une action.
// action : Action à exécuter pendant le test.
// saisie : Réponses fournies pendant le test.
string Capturer(Action action, string saisie = "")
{
    // Entrée de la console conservée pendant le test.
    TextReader entree = Console.In;
    // Texte affiché pendant le test.
    StringWriter texte = new StringWriter();
    Console.SetOut(texte);
    Console.SetIn(new StringReader(saisie));
    try
    {
        action();
        return texte.ToString();
    }
    finally
    {
        Console.SetOut(sortie);
        Console.SetIn(entree);
    }
}

// Exécute un test et conserve son résultat.
// nom : Nom du test.
// action : Action à exécuter pendant le test.
void Tester(string nom, Action action)
{
    new Parking();
    // Champ contenant la liste des tickets à remettre à zéro.
    FieldInfo champ = typeof(Ticket).GetField("_ticketList", BindingFlags.NonPublic | BindingFlags.Static)!;
    ((IList)champ.GetValue(null)!).Clear();
    try
    {
        action();
        reussis++;
        resultats.Add(new string[] { nom, "Réussi" });
        sortie.WriteLine("OK : " + nom);
    }
    // Erreur rencontrée lors de l’exécution du test.
    catch (Exception erreur)
    {
        echoues++;
        resultats.Add(new string[] { nom, "Échec" });
        sortie.WriteLine("ECHEC : " + nom + " - " + erreur.Message);
    }
}

Tester("Plaques valides", () =>
{
    Verifier(Vehicule.PlaqueEstValide("VD1"));
    Verifier(Vehicule.PlaqueEstValide("GE123456"));
    Verifier(Vehicule.PlaqueEstValide("vd123"));
});
Tester("Plaques invalides", () =>
{
    // Plaque saisie par l’utilisateur.
    foreach (string? plaque in new string?[] { null, "", "VD", "V123", "VDD123", "VD1234567", "12VD", "VD12!", "VD١" })
    {
        Verifier(!Vehicule.PlaqueEstValide(plaque));
    }
});
Tester("Plaque mise en majuscules", () => Verifier(new Vehicule(" vd123 ").Plaque == "VD123"));
Tester("Prix pour une heure", () => Verifier(Ticket.CalculerPrix(new DateTime(2026, 10, 30, 8, 0, 0), new DateTime(2026, 10, 30, 9, 0, 0)) == 3m));
Tester("Prix pour trente minutes", () => Verifier(Ticket.CalculerPrix(new DateTime(2026, 10, 30, 8, 0, 0), new DateTime(2026, 10, 30, 8, 30, 0)) == 1.50m));
Tester("Prix pour une durée nulle", () => Verifier(Ticket.CalculerPrix(DateTime.Today, DateTime.Today) == 0m));
Tester("Arrondi du prix", () => Verifier(Ticket.CalculerPrix(DateTime.Today, DateTime.Today.AddSeconds(6)) == 0.01m));
Tester("Durée négative refusée", () =>
{
    // Indique si la durée incorrecte a été refusée.
    bool refusee = false;
    try { Ticket.CalculerPrix(DateTime.Today, DateTime.Today.AddMinutes(-1)); }
    catch (ArgumentException) { refusee = true; }
    Verifier(refusee);
});
Tester("Place libre puis occupée", () =>
{
    // Place de parking en cours de traitement.
    PlaceParking place = new PlaceParking(1);
    Verifier(!place.EstOccupee);
    place.Vehicule = new Vehicule("VD1");
    Verifier(place.EstOccupee);
    place.Vehicule = null;
    Verifier(!place.EstOccupee);
});
Tester("Entrée et ticket", () =>
{
    // Texte affiché pendant le test.
    string texte = Capturer(() => Parking.EntreeVehicule("VD123"));
    Verifier(texte.Contains("VD123") && texte.Contains("01") && texte.Contains("3.00"));
    Verifier(Capturer(Parking.PlanParking).Contains("|01:X|"));
});
Tester("Plaque en double même avec des minuscules", () =>
{
    Capturer(() => Parking.EntreeVehicule("VD123"));
    Verifier(Capturer(() => Parking.EntreeVehicule("vd123")).Contains("déjà"));
    Verifier(Capturer(Parking.PlanParking).Contains("|02:L|"));
});
Tester("Entrée invalide refusée", () =>
{
    Verifier(Capturer(() => Parking.EntreeVehicule("123")).Contains("invalide"));
    Verifier(Capturer(Parking.PlanParking).Contains("|01:L|"));
});
Tester("Parking plein", () =>
{
    // Numéro de place utilisé dans la boucle.
    for (int i = 1; i <= 20; i++) { Capturer(() => Parking.EntreeVehicule("VD" + i)); }
    Verifier(Capturer(() => Parking.EntreeVehicule("VD21")).Contains("complet"));
});
Tester("Recherche par plaque", () =>
{
    Capturer(() => Parking.EntreeVehicule("VD123"));
    // Texte affiché pendant le test.
    string texte = Capturer(Parking.RechercheVehicule, "1\nvd123\n");
    Verifier(texte.Contains("VD123") && texte.Contains("Durée") && texte.Contains("Prix actuel"));
});
Tester("Recherche par place", () =>
{
    Capturer(() => Parking.EntreeVehicule("VD123"));
    Verifier(Capturer(Parking.RechercheVehicule, "2\n1\n").Contains("VD123"));
});
Tester("Recherche inconnue", () => Verifier(Capturer(Parking.RechercheVehicule, "2\n99\n").Contains("Aucun véhicule")));
Tester("Sortie annulée", () =>
{
    Capturer(() => Parking.EntreeVehicule("VD123"));
    Capturer(Parking.SortieVehicule, "2\n1\nn\n");
    Verifier(Capturer(Parking.PlanParking).Contains("|01:X|"));
    Verifier(!Capturer(Ticket.AfficherHistorique).Contains("SORTIE ("));
});
Tester("Sortie confirmée par place", () =>
{
    Capturer(() => Parking.EntreeVehicule("VD123"));
    Capturer(Parking.SortieVehicule, "2\n1\no\n");
    Verifier(Capturer(Parking.PlanParking).Contains("|01:L|"));
    Verifier(Capturer(Ticket.AfficherHistorique).Contains("SORTIE ("));
});
Tester("Sortie confirmée par plaque", () =>
{
    Capturer(() => Parking.EntreeVehicule("VD123"));
    Capturer(Parking.SortieVehicule, "1\nvd123\no\n");
    Verifier(Capturer(Parking.PlanParking).Contains("|01:L|"));
});
Tester("Place libérée réutilisée", () =>
{
    Capturer(() => Parking.EntreeVehicule("VD123"));
    Capturer(Parking.SortieVehicule, "2\n1\no\n");
    Verifier(Capturer(() => Parking.EntreeVehicule("GE456")).Contains("Place : 01"));
});
Tester("Statistiques occupation", () =>
{
    Capturer(() => Parking.EntreeVehicule("VD123"));
    // Texte affiché pendant le test.
    string texte = Capturer(Parking.StatistiquesDuJour);
    Verifier(texte.Contains("1 / 20") && texte.Contains("5.00 %") && texte.Contains("VD123"));
});
Tester("Paiements et historique", () =>
{
    // Heure de départ choisie pour le test.
    DateTime debut = new DateTime(2026, 10, 30, 8, 0, 0);
    Capturer(() => Ticket.CreerTicket("VD123", 1, debut));
    Ticket.FermerTicket("VD123", debut.AddHours(1));
    Ticket.FermerTicket("VD123", debut.AddHours(2));
    Verifier(Ticket.MontantTotalPaye() == 3m);
    // Texte affiché pendant le test.
    string texte = Capturer(Ticket.AfficherHistorique);
    Verifier(texte.Contains("ENTREE (") && texte.Contains("SORTIE (") && texte.Contains("3.00 CHF"));
});
Tester("Menu lettre sans quitter", () =>
{
    // Parking utilisé par le programme.
    Parking parking = new Parking();
    // Indique si le menu a demandé de quitter.
    bool quitte = true;
    Capturer(() => quitte = parking.ChoixMenu(), "a\n");
    Verifier(!quitte);
});
Tester("Menu zéro pour quitter", () =>
{
    // Parking utilisé par le programme.
    Parking parking = new Parking();
    // Indique si le menu a demandé de quitter.
    bool quitte = false;
    Capturer(() => quitte = parking.ChoixMenu(), "0\n");
    Verifier(quitte);
});

sortie.WriteLine($"{reussis} réussis, {echoues} échecs");
if (args.Length > 0)
{
    File.WriteAllText(args[0], System.Text.Json.JsonSerializer.Serialize(resultats));
}
Environment.ExitCode = echoues == 0 ? 0 : 1;
