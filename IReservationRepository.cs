using System;

public interface IReservationRepository
{
    void AddReservation(Reservation reservation);
    void DeleteReservation(string reserverName);
    IEnumerable<Reservation> GetAllReservations();
}
