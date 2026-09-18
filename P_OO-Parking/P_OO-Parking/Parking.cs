using System;

namespace P_OO_Parking
{
    class Parking
    {
        private const int MaxPlaces = 20;
        private static Vehicule?[] _places = new Vehicule?[MaxPlaces];

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
                    string plaqueRecherche = Vehicule.DemanderPlaque();
                        RechercheVehicule(plaqueRecherche);
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

            foreach (Vehicule? place in _places)
            {
                if (place != null && place.Plaque == plaque)
                {
                    Console.WriteLine("Ce véhicule est déjà dans le parking.");
                    return;
                }
            }

            int indexLibre = -1;
            int i = 0;
            foreach (Vehicule? place in _places)
            {
                if (place == null)
                {
                    indexLibre = i;
                    break;
                }
                i++;
            }

            if (indexLibre == -1)
            {
                Console.WriteLine("Parking complet, aucune place disponible.");
                return;
            }

            Vehicule vehicule = new Vehicule(plaque);
            vehicule.HeureEntree = DateTime.Now;
            _places[indexLibre] = vehicule;

            Console.WriteLine($"Véhicule {plaque}, garé à la place {indexLibre + 1}, à {vehicule.HeureEntree}.");
        }

        public static void SortieVehicule(string plaque)
        {
            int index = -1;
            int i = 0;

            foreach (Vehicule? place in _places)
            {
                if (place != null && place.Plaque == plaque)
                {
                    index = i;
                    break;
                }
                i++;
            }

            if (index == -1)
            {
                Console.WriteLine("Aucun véhicule trouvé avec cette plaque.");
                return;
            }

            Vehicule vehicule = _places[index]!;
            vehicule.HeureSortie = DateTime.Now;

            
            bool input = true;
            do
            {
                Console.Write("Confirmer la sortie ? (o/n) : ");
                string? reponse = Console.ReadLine();

                if (reponse == "o")
                {
                    _places[index] = null;
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
         
            int i = 1;
            int r = 1;
            foreach (Vehicule? place in _places)
            {
                
                string statut;
                if (place == null)
                {
                    statut = "L";
                }
                else
                {
                    statut = "X";
                }

                Console.Write($"|{i:D2}:{statut}| ");

                if (r == 5)
                {
                    Console.WriteLine();
                    r= 0;
                }

                i++;
                r++;
            }
        }




        public static void RechercheVehicule(string plaque) // to do recherche place 
        {
            int index = -1;
            int i = 0;


            foreach (Vehicule? place in _places)
            {
                if (place != null && place.Plaque == plaque)
                {
                    index = i;
                    Vehicule vehicule = new Vehicule(plaque);
                    Console.WriteLine($"Véhicule {plaque} garé à la place {index + 1}.");
                }
                i++;
            }

            if (index == -1)
            {
                Console.WriteLine("Aucun véhicule trouvé avec cette plaque.");
                return;
            }









        }















    }

















    
}