using System;
using System.Collections.Generic;

namespace P_OO_Parking
{
    class Ticket
    {
        private static List<Ticket> _ticketList = new List<Ticket>();

        public string Plaque { get; set; }
        public int Place { get; set; }
        public DateTime HeureEntree { get; set; }
        public DateTime HeureSortie { get; set; }
        public bool EstSorti { get; set; }

        public static void Haveticket(string plaque, int place, DateTime heure)
        {
            Ticket ticket = new Ticket();

            ticket.Plaque = plaque;
            ticket.Place = place;
            ticket.HeureEntree = heure;
            ticket.EstSorti = false;

            _ticketList.Add(ticket);

            Console.WriteLine($"Véhicule {plaque}, garé à la place {place}, à {heure}.");
        }

        public static void sortieticket(string plaque, DateTime heure)
        {
            foreach (Ticket ticket in _ticketList)
            {
                if (ticket.Plaque == plaque && ticket.EstSorti == false)
                {
                    ticket.HeureSortie = heure;
                    ticket.EstSorti = true;
                    TimeSpan duree = ticket.HeureSortie - ticket.HeureEntree;  // lien https://learn.microsoft.com/en-us/dotnet/api/system.timespan?view=net-10.0
                    double prix = duree.TotalMinutes * 1.00;
                    Console.WriteLine($"Plaque : {ticket.Plaque}");
                    Console.WriteLine($"Place : {ticket.Place}");
                    Console.WriteLine($"Entrée : {ticket.HeureEntree}");
                    Console.WriteLine($"Sortie : {ticket.HeureSortie}");
                    

                }

            }
        }
    }
}