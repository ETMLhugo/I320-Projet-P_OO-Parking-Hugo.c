using System;
using System.Collections.Generic;

namespace P_OO_Parking
{
    class Parking
    {
        // Nombre maximum de places dans le parking.
        private const int MaxPlaces = 20;
        // Liste des places du parking.
        private static List<PlaceParking> _places = new List<PlaceParking>();

        // Crée les vingt places du parking.
        public Parking()
        {
            _places.Clear();
            // Numéro de place utilisé dans la boucle.
            for (int i = 1; i <= MaxPlaces; i++)
            {
                _places.Add(new PlaceParking(i));
            }
        }

        // Affiche les choix du menu principal.
        public void AfficherMenu()
        {
            Console.WriteLine("=== MENU PRINCIPAL ===");
            Console.WriteLine("1. Entrée d'un véhicule");
            Console.WriteLine("2. Sortie d'un véhicule");
            Console.WriteLine("3. Afficher l'état du parking");
            Console.WriteLine("4. Rechercher un véhicule");
            Console.WriteLine("5. Statistiques du jour");
            Console.WriteLine("6. Historique des transactions");
            Console.WriteLine("0. Quitter");
            Console.Write("Votre choix : ");
        }

        // Lit le choix et lance l’action demandée.
        public bool ChoixMenu()
        {
            // Choix saisi dans le menu.
            int choix;
            if (!int.TryParse(Console.ReadLine(), out choix))
            {
                Console.WriteLine("Choix invalide.");
                return false;
            }
            switch (choix)
            {
                case 1:
                    // Plaque du véhicule qui entre.
                    string plaqueEntree = Vehicule.DemanderPlaque();
                    EntreeVehicule(plaqueEntree);
                    break;
                case 2:
                    SortieVehicule();
                    break;
                case 3:
                    PlanParking();
                    break;
                case 4:
                    RechercheVehicule();
                    break;
                case 5:
                    StatistiquesDuJour();
                    break;
                case 6:
                    Ticket.AfficherHistorique();
                    break;
                case 0:
                    return true;
                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }

            return false;
        }

        // Vérifie le véhicule et lui attribue une place libre.
        // plaque : Plaque saisie par l’utilisateur.
        public static void EntreeVehicule(string plaque)
        {
            if (!Vehicule.PlaqueEstValide(plaque))
            {
                Console.WriteLine("Plaque invalide.");
                return;
            }
            plaque = plaque.Trim().ToUpperInvariant();
            // Place de parking en cours de traitement.
            foreach (PlaceParking place in _places)
            {
                if (place.Vehicule != null && place.Vehicule.Plaque == plaque)
                {
                    Console.WriteLine("Ce véhicule est déjà dans le parking.");
                    return;
                }
            }

            // Première place libre trouvée.
            PlaceParking? placeLibre = null;
            // Place de parking en cours de traitement.
            foreach (PlaceParking place in _places)
            {
                if (place.EstOccupee == false)
                {
                    placeLibre = place;
                    break;
                }
            }

            if (placeLibre == null)
            {
                Console.WriteLine("Parking complet, aucune place disponible.");
                return;
            }

            // Véhicule en cours de traitement.
            Vehicule vehicule = new Vehicule(plaque);
            vehicule.HeureEntree = DateTime.Now;
            placeLibre.Vehicule = vehicule;

            Ticket.CreerTicket(plaque, placeLibre.Numero, vehicule.HeureEntree);
        }

        // Retrouve une place à partir de la plaque ou du numéro.
        public static PlaceParking? ChoisirPlace()
        {
            // Choix saisi dans le menu.
            int choix;
            Console.Clear();
            Console.WriteLine("Choisissez comment trouver le véhicule :");
            Console.WriteLine("1 : Par plaque d'immatriculation");
            Console.WriteLine("2 : Par numéro de place");
            Console.Write("Votre choix : ");
            int.TryParse(Console.ReadLine(), out choix);

            if (choix == 1)
            {
                // Plaque saisie par l’utilisateur.
                string plaque = Vehicule.DemanderPlaque();

                // Place de parking en cours de traitement.
                foreach (PlaceParking place in _places)
                {
                    if (place.Vehicule != null && place.Vehicule.Plaque == plaque)
                    {
                        return place;
                    }
                }
            }
            else if (choix == 2)
            {
                // Numéro de place saisi.
                int numero;
                Console.Write("Entrez le numéro de place : ");
                int.TryParse(Console.ReadLine(), out numero);

                // Place de parking en cours de traitement.
                foreach (PlaceParking place in _places)
                {
                    if (place.Numero == numero && place.Vehicule != null)
                    {
                        return place;
                    }
                }
            }
            else
            {
                Console.WriteLine("Choix invalide.");
            }

            return null;
        }

        // Affiche le montant et demande confirmation avant de libérer la place.
        public static void SortieVehicule()
        {
            // Place de parking en cours de traitement.
            PlaceParking? place = ChoisirPlace();

            if (place == null || place.Vehicule == null)
            {
                Console.WriteLine("Aucun véhicule trouvé.");
                return;
            }

            // Véhicule en cours de traitement.
            Vehicule vehicule = place.Vehicule;
            // Heure retenue pour la sortie.
            DateTime heureSortie = DateTime.Now;

            // Temps passé dans le parking.
            TimeSpan duree = heureSortie - vehicule.HeureEntree;
            // Montant à payer pour le stationnement.
            decimal montant = Ticket.CalculerPrix(vehicule.HeureEntree, heureSortie);

            Console.WriteLine("=== SORTIE ===");
            Console.WriteLine($"Plaque : {vehicule.Plaque}");
            Console.WriteLine($"Place : {place.Numero:D2}");
            Console.WriteLine($"Heure d'entrée : {vehicule.HeureEntree}");
            Console.WriteLine($"Heure de sortie : {heureSortie}");
            Console.WriteLine($"Durée : {(int)duree.TotalHours} h {duree.Minutes} min {duree.Seconds} s");
            Console.WriteLine($"Montant à payer : {montant:F2} CHF");

            // Indique si la réponse saisie est valable.
            bool input = true;
            do
            {
                Console.Write("Confirmer la sortie ? (o/n) : ");
                // Réponse donnée pour confirmer la sortie.
                string reponse = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

                if (reponse == "o")
                {
                    vehicule.HeureSortie = heureSortie;
                    Ticket.FermerTicket(vehicule.Plaque, heureSortie);
                    place.Vehicule = null;
                    Console.WriteLine("Place libérée.");
                    input = true;
                }
                else if (reponse == "n")
                {
                    Console.WriteLine("Sortie annulée.");
                    input = true;
                }
                else
                {
                    Console.WriteLine("Entrée non valable.");
                    input = false;
                }
            } while (input == false);
        }

        // Affiche les places libres et occupées.
        public static void PlanParking()
        {
            Console.Clear();
            Console.WriteLine("=== PLAN DU PARKING ===");
            Console.WriteLine("L : Libre / X : Occupée");
            Console.WriteLine();

            // Nombre de places affichées sur la ligne.
            int ligne = 0;

            // Place de parking en cours de traitement.
            foreach (PlaceParking place in _places)
            {
                // Lettre affichée pour une place libre ou occupée.
                string statut;
                if (place.EstOccupee == true)
                {
                    statut = "X";
                }
                else
                {
                    statut = "L";
                }

                Console.Write($"|{place.Numero:D2}:{statut}| "); 
                ligne++;

                if (ligne == 5)
                {
                    Console.WriteLine();
                    ligne = 0;
                }
            }
        }

        // Affiche les informations du véhicule recherché.
        public static void RechercheVehicule()
        {
            // Place de parking en cours de traitement.
            PlaceParking? place = ChoisirPlace();

            if (place == null || place.Vehicule == null)
            {
                Console.WriteLine("Aucun véhicule trouvé.");
                return;
            }

            // Véhicule en cours de traitement.
            Vehicule vehicule = place.Vehicule;
            // Heure actuelle utilisée pour le calcul.
            DateTime maintenant = DateTime.Now;
            // Temps passé dans le parking.
            TimeSpan duree = maintenant - vehicule.HeureEntree;
            // Prix calculé pour la durée de stationnement.
            decimal prix = Ticket.CalculerPrix(vehicule.HeureEntree, maintenant);

            Console.WriteLine("=== TICKET ===");
            Console.WriteLine($"Plaque : {vehicule.Plaque}");
            Console.WriteLine($"Place : {place.Numero:D2}");
            Console.WriteLine($"Heure d'entrée : {vehicule.HeureEntree}");
            Console.WriteLine($"Durée : {(int)duree.TotalHours} h {duree.Minutes} min {duree.Seconds} s");
            Console.WriteLine($"Prix actuel : {prix:F2} CHF");
        }

        // Affiche l’occupation du parking et les montants payés.
        public static void StatistiquesDuJour()
        {
            Console.Clear();
            Console.WriteLine("=== STATISTIQUES DU JOUR ===");

            // Nombre de places occupées.
            int occupees = 0;
            // Place de parking en cours de traitement.
            foreach (PlaceParking place in _places)
            {
                if (place.EstOccupee == true)
                {
                    occupees++;
                }
            }

            // Pourcentage des places occupées.
            double pourcentage = occupees * 100.0 / MaxPlaces;
            // Somme des montants payés.
            decimal total = Ticket.MontantTotalPaye();

            Console.WriteLine($"Places occupées : {occupees} / {MaxPlaces}");
            Console.WriteLine($"Taux d'occupation : {pourcentage:F2} %");
            Console.WriteLine($"Montant total payé : {total:F2} CHF");
            Console.WriteLine();
            Console.WriteLine("Véhicules encore garés :");

            if (occupees == 0)
            {
                Console.WriteLine("Aucun véhicule dans le parking.");
            }

            // Heure actuelle utilisée pour le calcul.
            DateTime maintenant = DateTime.Now;
            // Place de parking en cours de traitement.
            foreach (PlaceParking place in _places)
            {
                if (place.Vehicule != null)
                {
                    // Temps passé dans le parking.
                    TimeSpan duree = maintenant - place.Vehicule.HeureEntree;
                    Console.WriteLine($"{place.Vehicule.Plaque} - Place {place.Numero:D2} - {(int)duree.TotalHours} h {duree.Minutes} min {duree.Seconds} s");
                }
            }
        }
    }
}
