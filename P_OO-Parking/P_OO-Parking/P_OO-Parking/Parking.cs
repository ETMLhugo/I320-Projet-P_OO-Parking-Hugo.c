using System;
using System.Collections.Generic;

namespace P_OO_Parking
{
    class Parking
    {
        private const int MaxPlaces = 20;
        private static List<PlaceParking> _places = new List<PlaceParking>();

        public Parking()
        {
            for (int i = 1; i <= MaxPlaces; i++)
            {
                _places.Add(new PlaceParking(i));
            }
        }

        public void Showmenu()
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

        public bool ChoixMenu()
        {
            int choix;
            int.TryParse(Console.ReadLine(), out choix);
            switch (choix)
            {
                case 1:
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

        public static void EntreeVehicule(string plaque)
        {
            foreach (PlaceParking place in _places)
            {
                if (place.Vehicule != null && place.Vehicule.Plaque == plaque)
                {
                    Console.WriteLine("Ce véhicule est déjà dans le parking.");
                    return;
                }
            }

            PlaceParking? placeLibre = null;
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

            Vehicule vehicule = new Vehicule(plaque);
            vehicule.HeureEntree = DateTime.Now;
            placeLibre.Vehicule = vehicule;

            Ticket.Haveticket(plaque, placeLibre.Numero, vehicule.HeureEntree);
        }

        public static PlaceParking? ChoisirPlace()
        {
            int choix;
            Console.Clear();
            Console.WriteLine("Choisissez comment trouver le véhicule :");
            Console.WriteLine("1 : Par plaque d'immatriculation");
            Console.WriteLine("2 : Par numéro de place");
            Console.Write("Votre choix : ");
            int.TryParse(Console.ReadLine(), out choix);

            if (choix == 1)
            {
                string plaque = Vehicule.DemanderPlaque();

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
                int numero;
                Console.Write("Entrez le numéro de place : ");
                int.TryParse(Console.ReadLine(), out numero);

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

        public static void SortieVehicule()
        {
            PlaceParking? place = ChoisirPlace();

            if (place == null || place.Vehicule == null)
            {
                Console.WriteLine("Aucun véhicule trouvé.");
                return;
            }

            Vehicule vehicule = place.Vehicule;
            vehicule.HeureSortie = DateTime.Now;

            TimeSpan duree = vehicule.HeureSortie - vehicule.HeureEntree;
            double montant = Ticket.CalculerPrix(vehicule.HeureEntree, vehicule.HeureSortie);

            Console.WriteLine("=== SORTIE ===");
            Console.WriteLine($"Plaque : {vehicule.Plaque}");
            Console.WriteLine($"Place : {place.Numero:D2}");
            Console.WriteLine($"Heure d'entrée : {vehicule.HeureEntree}");
            Console.WriteLine($"Heure de sortie : {vehicule.HeureSortie}");
            Console.WriteLine($"Durée : {duree.Hours} h {duree.Minutes} min {duree.Seconds} s");
            Console.WriteLine($"Montant à payer : {montant:F2} CHF");

            bool input = true;
            do
            {
                Console.Write("Confirmer la sortie ? (o/n) : ");
                string? reponse = Console.ReadLine();

                if (reponse == "o")
                {
                    Ticket.sortieticket(vehicule.Plaque, vehicule.HeureSortie);
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

        public static void PlanParking()
        {
            Console.Clear();
            Console.WriteLine("=== PLAN DU PARKING ===");
            Console.WriteLine("L : Libre / X : Occupée");
            Console.WriteLine();

            int ligne = 0;

            foreach (PlaceParking place in _places)
            {
                string statut;
                if (place.EstOccupee == true)
                {
                    statut = "X";
                }
                else
                {
                    statut = "L";
                }

                Console.Write($"|{place.Numero:D2}:{statut}| "); // lien i;D2 https://stackoverflow.com/questions/5972949/number-formatting-how-to-convert-1-to-01-2-to-02-etc
                ligne++;

                if (ligne == 5)
                {
                    Console.WriteLine();
                    ligne = 0;
                }
            }
        }

        public static void RechercheVehicule()
        {
            PlaceParking? place = ChoisirPlace();

            if (place == null || place.Vehicule == null)
            {
                Console.WriteLine("Aucun véhicule trouvé.");
                return;
            }

            Vehicule vehicule = place.Vehicule;
            DateTime maintenant = DateTime.Now;
            TimeSpan duree = maintenant - vehicule.HeureEntree;
            double prix = Ticket.CalculerPrix(vehicule.HeureEntree, maintenant);

            Console.WriteLine("=== TICKET ===");
            Console.WriteLine($"Plaque : {vehicule.Plaque}");
            Console.WriteLine($"Place : {place.Numero:D2}");
            Console.WriteLine($"Heure d'entrée : {vehicule.HeureEntree}");
            Console.WriteLine($"Durée : {duree.Hours} h {duree.Minutes} min {duree.Seconds} s");
            Console.WriteLine($"Prix actuel : {prix:F2} CHF");
        }

        public static void StatistiquesDuJour()
        {
            Console.Clear();
            Console.WriteLine("=== STATISTIQUES DU JOUR ===");

            int occupees = 0;
            foreach (PlaceParking place in _places)
            {
                if (place.EstOccupee == true)
                {
                    occupees++;
                }
            }

            double pourcentage = occupees * 100.0 / MaxPlaces;
            double total = Ticket.MontantTotalPaye();

            Console.WriteLine($"Places occupées : {occupees} / {MaxPlaces}");
            Console.WriteLine($"Taux d'occupation : {pourcentage:F2} %");
            Console.WriteLine($"Montant total payé : {total:F2} CHF");
            Console.WriteLine();
            Console.WriteLine("Véhicules encore garés :");

            if (occupees == 0)
            {
                Console.WriteLine("Aucun véhicule dans le parking.");
            }

            DateTime maintenant = DateTime.Now;
            foreach (PlaceParking place in _places)
            {
                if (place.Vehicule != null)
                {
                    TimeSpan duree = maintenant - place.Vehicule.HeureEntree;
                    Console.WriteLine($"{place.Vehicule.Plaque} - Place {place.Numero:D2} - {duree.Hours} h {duree.Minutes} min {duree.Seconds} s");
                }
            }
        }
    }
}
