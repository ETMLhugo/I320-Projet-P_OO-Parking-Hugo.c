using System;
using System.Text.RegularExpressions;

namespace P_OO_Parking
{
    class Vehicule
    {
        // Numéro qui sera donné au prochain véhicule.
        private static int _prochainVid = 1;

        // Identifiant du véhicule.
        private int _vehiculeId;

        // Plaque d’immatriculation du véhicule.
        public string Plaque { get; }

        // Date et heure d’entrée dans le parking.
        public DateTime HeureEntree { get; set; }
        // Date et heure de sortie du parking.
        public DateTime HeureSortie{ get; set; }

        // Identifiant du véhicule.
        public int Vid
        {
            get { return _vehiculeId; }
        }

        // Crée un véhicule avec sa plaque et son identifiant.
        // plaque : Plaque saisie par l’utilisateur.
        public Vehicule(string plaque)
        {

            Plaque = plaque.Trim().ToUpperInvariant();
            _vehiculeId = _prochainVid;
            _prochainVid++;
        }

        // Vérifie que la plaque contient deux lettres et un à six chiffres.
        // plaque : Plaque saisie par l’utilisateur.
        public static bool PlaqueEstValide(string? plaque)
        {
            return !string.IsNullOrWhiteSpace(plaque) && Regex.IsMatch(plaque.Trim(), @"^[A-Za-z]{2}[0-9]{1,6}$"); 

        }

        // Demande une plaque jusqu’à obtenir un format valable.
        public static string DemanderPlaque()
        {
            // Plaque saisie par l’utilisateur.
            string plaque;
            Console.Clear();
            do
            {
                
                Console.Write("Entrez votre plaque d'immatriculation : ");
                plaque = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

                if (!Vehicule.PlaqueEstValide(plaque))
                {
                    Console.WriteLine("Plaque invalide. Format attendu : 2 lettres + jusqu'à 6 chiffres (ex: VD123456)");
                }

            } while (!Vehicule.PlaqueEstValide(plaque));

            return plaque;
        }

    }

}
