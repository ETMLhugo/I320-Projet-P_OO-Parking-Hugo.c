using System;
using System.Text.RegularExpressions;

namespace P_OO_Parking
{
    class Vehicule
    {
        private static int _prochainVid = 1;

        private int _vehiculeId;

        public string Plaque { get; }

        public DateTime HeureEntree { get; set; }
        public DateTime HeureSortie{ get; set; }

        public int Vid
        {
            get { return _vehiculeId; }
        }

        public Vehicule(string plaque)
        {

            Plaque = plaque;
            _vehiculeId = _prochainVid;
            _prochainVid++;
        }

        public static bool PlaqueEstValide(string plaque)
        {
            return Regex.IsMatch(plaque, @"^[A-Za-z]{2}\d{1,6}$");
    
        
        }

        public static string DemanderPlaque()
        {
            string plaque;
            Console.Clear();
            do
            {
                
                Console.Write("Entrez votre plaque d'immatriculation : ");
                plaque = Console.ReadLine();

                if (!Vehicule.PlaqueEstValide(plaque))
                {
                    Console.WriteLine("Plaque invalide. Format attendu : 2 lettres + jusqu'à 6 chiffres (ex: VD123456)");
                }

            } while (!Vehicule.PlaqueEstValide(plaque));

            return plaque;
        }


        


    }







}