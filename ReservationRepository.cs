using System;
using System.Collections.Generic;

public class ReservationRepository : IReservationRepository
{
    private readonly List<Reservation> reservations = new List<Reservation>();

    public void AddReservation(Reservation reservation)
    {
        reservations.Add(reservation);
    }

    public void DeleteReservation(string reserverName)
    {
        reservations.RemoveAll(r => r.ReserverName == reserverName);
    }

    public IEnumerable<Reservation> GetAllReservations()
    {
        return reservations;
    }
}