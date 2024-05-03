using System;
using System.Collections.Generic;

public class ReservationService
{
    private readonly ReservationHandler _reservationHandler;
    private readonly IReservationRepository _reservationRepository;
    private readonly Reservation reservation1;

    public ReservationService(ReservationHandler reservationHandler, IReservationRepository reservationRepository)
    {
        _reservationHandler = reservationHandler;
        _reservationRepository = reservationRepository;
    }

    public bool AddReservation(string day, string roomNumber, string reserverName, DateTime enterTime)
    {
        try
        {
            _reservationHandler.AddReservation(day, roomNumber, reserverName, enterTime);
            _reservationRepository.AddReservation(reservation1);
            return true; // Successfully added reservation
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to add reservation: {ex.Message}");
            return false; // Reservation addition failed
        }
    }

    public bool DeleteReservationByName(string reserverName, string roomNumber, string day, DateTime enterTime)
    {
        try
        {
            _reservationHandler.DeleteReservationByName(reserverName, roomNumber, day, enterTime);
            _reservationRepository.DeleteReservation(reserverName);
            return true; // Successfully deleted reservation(s)
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to delete reservation(s): {ex.Message}");
            return false; // Deletion failed
        }
    }

    public void PrintWeeklySchedule()
    {
        _reservationHandler.PrintWeeklySchedule();
    }
}
