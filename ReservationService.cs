using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

public class ReservationService
{
    private readonly ReservationHandler _reservationHandler;
    private readonly IReservationRepository _reservationRepository;
    private readonly Reservation reservation1;
    private static List<Reservation>? _reservations;

    public static void InitializeReservations(string jsonFilePath){
        try{
            string jsonString = File.ReadAllText(jsonFilePath);
            _reservations = JsonSerializer.Deserialize<List<Reservation>>(
            jsonString,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
            ) ?? new List<Reservation>();
        }
        catch(JsonException ex){
            Console.WriteLine("An error occured while processing the Json file" + ex.Message);
        }
        catch(FileNotFoundException ex){
            Console.WriteLine("Json File not found." + ex.Message);
        }
        catch(Exception ex){
            Console.WriteLine("An unexpected error occured." + ex.Message);
        }   
        
    }
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
            return true; 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to add reservation: {ex.Message}");
            return false; 
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
        var reservations = GetAllReservations();
        if (reservations == null || !reservations.Any())
        {
            Console.WriteLine("No reservations loaded.");
            return;
        }

        Console.WriteLine("Weekly Schedule:");
        for (int i = 0; i < 7; i++)
        {
            DayOfWeek dayOfWeek = (DayOfWeek)(((int)DayOfWeek.Monday + i) % 7);
            string dayOfWeekString = dayOfWeek.ToString();
            Console.WriteLine($"Day: {dayOfWeekString}");

            var dayReservations = reservations.Where(r => r.Day.Equals(dayOfWeekString, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!dayReservations.Any())
            {
                Console.WriteLine("No reservations for this day.");
                continue;
            }

            foreach (var reservation in dayReservations)
            {
                Console.WriteLine($"Room {reservation.RoomNumber}: {reservation.EnterTime} - {reservation.ReserverName}");
            }
        }
    }


    
    public List<Reservation> GetReservationsByReserverName(string name)
    {
        if (_reservations == null)
        {
            Console.WriteLine("Reservation list is null.");
            return new List<Reservation>();
        }

        var matchedReservations = _reservations.Where(r => string.Equals(r.ReserverName, name, StringComparison.OrdinalIgnoreCase)).ToList();

        if (matchedReservations.Count == 0)
        {
            Console.WriteLine($"No reservations found for reserver: {name}");
        }
        return matchedReservations;
    }


    public List<Reservation> GetAllReservations()
    {
        try
        {
            string jsonData = File.ReadAllText("ReservationData.json");
            var reservations = JsonSerializer.Deserialize<List<Reservation>>(jsonData);
            if (reservations == null)
            {
                Console.WriteLine("No reservations found in the file.");
                return new List<Reservation>();
            }
            return reservations;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading or parsing reservations: {ex.Message}");
            return new List<Reservation>();
        }
    }

    
}