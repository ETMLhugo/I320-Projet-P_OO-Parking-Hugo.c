using System;

namespace P_OO_Parking
{
    class PlaceParking
    {
        // Numéro de la place de parking.
        public int Numero { get; set; }

        // Véhicule présent sur cette place.
        public Vehicule? Vehicule { get; set; }

        // Indique si la place contient un véhicule.
        public bool EstOccupee
        {
            get { return Vehicule != null; }
        }

        // Crée une place avec son numéro.
        // numero : Numéro de place saisi.
        public PlaceParking(int numero)
        {
            Numero = numero;
            Vehicule = null;
        }
    }
}
