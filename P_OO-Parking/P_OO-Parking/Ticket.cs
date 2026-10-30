using System;
using System.Collections.Generic;

namespace P_OO_Parking
{
    class Ticket
    {
        // Prix payé pour une heure de stationnement.
        public const decimal TarifHoraire = 1.00m;

        // Liste des tickets créés pendant la session.
        private static List<Ticket> _ticketList = new List<Ticket>();

        // Plaque d’immatriculation du véhicule.
        public string Plaque { get; set; } = "";
        // Numéro de la place attribuée.
        public int Place { get; set; }
        // Date et heure d’entrée dans le parking.
        public DateTime HeureEntree { get; set; }
        // Date et heure de sortie du parking.
        public DateTime HeureSortie { get; set; }
        // Montant payé pour le stationnement.
        public decimal Montant { get; set; }
        // Indique si le véhicule a quitté le parking.
        public bool EstSorti { get; set; }

        // Crée et affiche le ticket lors d’une entrée.
        // plaque : Plaque saisie par l’utilisateur.
        // place : Place de parking en cours de traitement.
        // heure : Heure à enregistrer sur le ticket.
        public static void CreerTicket(string plaque, int place, DateTime heure)
        {
            // Ticket en cours de traitement.
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

        // Calcule le prix du stationnement et l’arrondit à deux décimales.
        // heureEntree : Heure d’entrée utilisée pour le calcul.
        // heureSortie : Heure retenue pour la sortie.
        public static decimal CalculerPrix(DateTime heureEntree, DateTime heureSortie)
        {
            // Temps passé dans le parking.
            TimeSpan duree = heureSortie - heureEntree; 
            if (duree.TotalSeconds < 0)
            {
                throw new ArgumentException("La sortie doit être après l’entrée.");
            }
            // Prix calculé pour la durée de stationnement.
            decimal prix = (decimal)duree.TotalHours * TarifHoraire;
            return Math.Round(prix, 2, MidpointRounding.AwayFromZero);
        }

        // Enregistre la sortie et le montant payé.
        // plaque : Plaque saisie par l’utilisateur.
        // heure : Heure à enregistrer sur le ticket.
        public static void FermerTicket(string plaque, DateTime heure)
        {
            // Ticket en cours de traitement.
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

        // Additionne les montants des tickets terminés.
        public static decimal MontantTotalPaye()
        {
            // Somme des montants payés.
            decimal total = 0;

            // Ticket en cours de traitement.
            foreach (Ticket ticket in _ticketList)
            {
                if (ticket.EstSorti == true)
                {
                    total = total + ticket.Montant;
                }
            }

            return total;
        }

        // Affiche les entrées et les sorties enregistrées.
        public static void AfficherHistorique()
        {
            Console.Clear();
            Console.WriteLine("=== HISTORIQUE DES TRANSACTIONS ===");

            if (_ticketList.Count == 0)
            {
                Console.WriteLine("Aucune transaction.");
            }

            // Ticket en cours de traitement.
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
