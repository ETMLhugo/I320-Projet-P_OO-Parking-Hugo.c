using System;
using System.Collections.Generic;

namespace P_OO_Parking
{
    class Ticket
    {
        public const double TarifHoraire = 3.00;

        private static List<Ticket> _ticketList = new List<Ticket>();

        public string Plaque { get; set; }
        public int Place { get; set; }
        public DateTime HeureEntree { get; set; }
        public DateTime HeureSortie { get; set; }
        public double Montant { get; set; }
        public bool EstSorti { get; set; }

        public static void Haveticket(string plaque, int place, DateTime heure)
        {
            Ticket ticket = new Ticket();
            ticket.Plaque = plaque;
            ticket.Place = place;
            ticket.HeureEntree = heure;
            ticket.EstSorti = false;

            _ticketList.Add(ticket);

            Console.WriteLine("=== TICKET ===");
            Console.WriteLine($"Plaque : {plaque}");
            Console.WriteLine($"Place : {place:D2}");
            Console.WriteLine($"Heure d'entrée : {heure}");
            Console.WriteLine($"Tarif horaire : {TarifHoraire:F2} CHF / heure");
        }

        public static double CalculerPrix(DateTime heureEntree, DateTime heureSortie)
        {
            TimeSpan duree = heureSortie - heureEntree; // lien https://learn.microsoft.com/en-us/dotnet/api/system.timespan?view=net-10.0
            double prix = duree.TotalHours * TarifHoraire;
            return prix;
        }

        public static void sortieticket(string plaque, DateTime heure)
        {
            foreach (Ticket ticket in _ticketList)
            {
                if (ticket.Plaque == plaque && ticket.EstSorti == false)
                {
                    ticket.HeureSortie = heure;
                    ticket.Montant = CalculerPrix(ticket.HeureEntree, heure);
                    ticket.EstSorti = true;
                }
            }
        }

        public static double MontantTotalPaye()
        {
            double total = 0;

            foreach (Ticket ticket in _ticketList)
            {
                if (ticket.EstSorti == true)
                {
                    total = total + ticket.Montant;
                }
            }

            return total;
        }

        public static void AfficherHistorique()
        {
            Console.Clear();
            Console.WriteLine("=== HISTORIQUE DES TRANSACTIONS ===");

            if (_ticketList.Count == 0)
            {
                Console.WriteLine("Aucune transaction.");
            }

            foreach (Ticket ticket in _ticketList)
            {
                Console.WriteLine($"ENTREE ({ticket.HeureEntree}) - {ticket.Plaque} (place {ticket.Place:D2})");

                if (ticket.EstSorti == true)
                {
                    Console.WriteLine($"SORTIE ({ticket.HeureSortie}) - {ticket.Plaque} (place {ticket.Place:D2}, {ticket.Montant:F2} CHF)");
                }
            }
        }
    }
}
