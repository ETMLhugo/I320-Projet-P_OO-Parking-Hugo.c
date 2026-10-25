using System;

namespace P_OO_Parking
{
    class PlaceParking
    {
        public int Numero { get; set; }

        public Vehicule Vehicule { get; set; }

        public bool EstOccupee
        {
            get { return Vehicule != null; }
        }

        public PlaceParking(int numero)
        {
            Numero = numero;
            Vehicule = null;
        }
    }
}
