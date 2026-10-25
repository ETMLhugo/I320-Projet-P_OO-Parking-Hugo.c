using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace P_OO_Parking
{
    class Parking
    {
        private const int MaxPlaces = 20;
        private static List<Vehicule> _places = new List<Vehicule>();

        public void Showmenu()
        {
            Console.WriteLine("=== MENU PRINCIPAL ===");
            Console.WriteLine("1. Entrée d'un véhicule");
            Console.WriteLine("2. Sortie d'un véhicule");
            Console.WriteLine("3. Afficher l'état du parking");
            Console.WriteLine("4. Rechercher un véhicule");
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
                    string plaqueSortie = Vehicule.DemanderPlaque();
                    SortieVehicule(plaqueSortie);
                    break;
                case 3:
                    PlanParking();
                    break;
                case 4:
                        RechercheVehicule();
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
            if (_places.Any(place => place.Plaque == plaque))
            {
                Console.WriteLine("Ce véhicule est déjà dans le parking.");
                return;
            }

            if (_places.Count >= MaxPlaces)
            {
                Console.WriteLine("Parking complet, aucune place disponible.");
                return;
            }

            int numeroPlace = 0;

            for (int i = 1; i <= MaxPlaces; i++)
            {
                bool occupe = false;

                foreach (Vehicule voiture in _places)
                {
                    if (voiture.Place == i)
                    {
                        occupe = true;
                    }
                }
                if (occupe == false && numeroPlace == 0)
                {
                    numeroPlace = i;
                }
            }

            Vehicule vehicule = new Vehicule(plaque);
            vehicule.Place = numeroPlace;
            vehicule.HeureEntree = DateTime.Now;

            _places.Add(vehicule);

            Ticket.Haveticket(plaque, vehicule.Place, vehicule.HeureEntree);


        }

        public static void SortieVehicule(string plaque)
        {
            Vehicule vehicule = _places.FirstOrDefault(place => place.Plaque == plaque);

            if (vehicule == null)
            {
                Console.WriteLine("Aucun véhicule trouvé avec cette plaque.");
                return;
            }

            vehicule.HeureSortie = DateTime.Now;

            bool input = true;
            do
            {
                Console.Write("Confirmer la sortie ? (o/n) : ");
                string reponse = Console.ReadLine();

                if (reponse == "o")
                {
                    vehicule.HeureSortie = DateTime.Now;
                    Ticket.sortieticket(plaque, vehicule.HeureSortie);

                    _places.Remove(vehicule);

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
            }while (input == false);
        }
      
            public static void PlanParking()
        {
            Console.Clear();
            Console.WriteLine("=== PLAN DU PARKING ===");
            Console.WriteLine("L : Libre / X : Occupée");
            Console.WriteLine();

            int ligne = 0;
            for (int i = 1; i <= MaxPlaces; i++)
            {
                bool occupe = false;
                foreach (Vehicule vehicule in _places)
                {
                    if (vehicule.Place == i)
                    {
                        occupe = true;
                    }
                }
                string statut;
                if (occupe == true)
                {
                    statut = "X";
                }
                else
                {
                    statut = "L";
                }
                Console.Write($"|{i:D2}:{statut}| "); // lien https://stackoverflow.com/questions/5972949/number-formatting-how-to-convert-1-to-01-2-to-02-etc
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
            bool trouver = false;
            int choix;

            Console.Clear();
            Console.WriteLine("Choisissez comment rechercher votre véhicule :");
            Console.WriteLine("1 : Par plaque d'immatriculation");
            Console.WriteLine("2 : Par place de parking");
            Console.Write("Votre choix : ");
            int.TryParse(Console.ReadLine(), out choix);

            switch (choix)
            {
                case 1:
                    string plaque = Vehicule.DemanderPlaque();

                    foreach (Vehicule vehicule in _places)
                    {
                        if (vehicule.Plaque == plaque)
                        {
                            trouver = true;
                            TimeSpan duree = DateTime.Now - vehicule.HeureEntree;
                            double prix = duree.TotalMinutes * 1.00;

                            

                            Console.WriteLine($"Véhicule {vehicule.Plaque}, garé à la place {vehicule.Place}, à {vehicule.HeureEntree}.");
                            Console.WriteLine($"Prix actuel : {prix} euros");
                        }
                    }
                    if (trouver == false)
                    {
                        Console.WriteLine("Aucun véhicule trouvé avec cette plaque.");
                    }
                    break;

                case 2:
                    int placeParking;

                    Console.Write("Entrez votre place de parking : ");
                    int.TryParse(Console.ReadLine(), out placeParking);

                    foreach (Vehicule vehicule in _places)
                    {
                        if (vehicule.Place == placeParking)
                        {
                            trouver = true;
                            TimeSpan duree = DateTime.Now - vehicule.HeureEntree;
                            double prix = duree.TotalMinutes * 1.00;



                            Console.WriteLine($"Véhicule {vehicule.Plaque}, garé à la place {vehicule.Place}, à {vehicule.HeureEntree}.");
                            Console.WriteLine($"Prix actuel : {prix} euros");
                        }
                    }

                    if (trouver == false)
                    {
                        Console.WriteLine("Aucun véhicule trouvé à cette place.");
                    }
                    break;

                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }
        }













    }















    

















    
}