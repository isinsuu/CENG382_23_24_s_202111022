using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class ReservationHandler
{
    private IReservationRepository _reservationRepository;
    private LogHandler _logHandler;
    private Dictionary<string, Dictionary<Room, List<(DateTime, string)>>> weeklyReservations;
    private List<Reservation> _reservations; // List to hold reservation data for JSON serialization
    private string _reservationDataFilePath = "ReservationData.json"; // File path for JSON data
    private TimeSpan breakTime = TimeSpan.FromMinutes(40);

    public ReservationHandler(RoomData roomData, IReservationRepository reservationRepository, LogHandler logHandler)
    {
        _reservationRepository = reservationRepository;
        _logHandler = logHandler;
        weeklyReservations = new Dictionary<string, Dictionary<Room, List<(DateTime, string)>>>();
        _reservations = ReadReservationsFromFile(); // Initialize the list from file

        foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
        {
            weeklyReservations[day.ToString()] = new Dictionary<Room, List<(DateTime, string)>>();
        }

        foreach (Room room in roomData.Rooms)
        {
            for (int i = 0; i < 7; i++)
            {
                DayOfWeek day = (DayOfWeek)(((int)DayOfWeek.Monday + i) % 7);
                weeklyReservations[day.ToString()][room] = new List<(DateTime, string)>();
            }
        }
    }

    private List<Reservation> ReadReservationsFromFile()
    {
        // Check if the file exists or if it is empty
        if (!File.Exists(_reservationDataFilePath) || new FileInfo(_reservationDataFilePath).Length == 0)
        {
            // Initialize with an empty list of reservations and write to file
            var emptyList = new List<Reservation>();
            UpdateReservationDataFile(emptyList); // Ensure the file is not just created but has valid JSON.
            return emptyList;
        }

        string json = File.ReadAllText(_reservationDataFilePath);
        var reservations = JsonSerializer.Deserialize<List<Reservation>>(json);
        
        // Ensure that deserialization results in a valid object list, not null
        return reservations ?? new List<Reservation>();
    }

    private void UpdateReservationDataFile(List<Reservation> reservations = null)
    {
        reservations ??= _reservations;  // Use passed reservations or fall back to existing field
        var reservationsJson = JsonSerializer.Serialize(reservations, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_reservationDataFilePath, reservationsJson);
    }


    public void AddReservation(string day, string roomNumber, string reserverName, DateTime enterTime)
    {
        Room room = Array.Find(weeklyReservations[day].Keys.ToArray(), r => r.RoomId == roomNumber);
        List<(DateTime, string)> roomReservations = weeklyReservations[day][room];
        DateTime endTime = enterTime.AddMinutes(40);

        if (roomReservations.Any(reservation => enterTime < reservation.Item1.Add(breakTime) && endTime > reservation.Item1))
        {
            Console.WriteLine("Reservation overlap detected. Choose another time.");
            return;
        }

        roomReservations.Add((enterTime, reserverName));
        _reservations.Add(new Reservation { Day = day, RoomNumber = roomNumber, ReserverName = reserverName, EnterTime = enterTime }); // Add to the JSON list
        UpdateReservationDataFile(); // Update JSON file

        Console.WriteLine($"Reservation added on {day} at {enterTime:hh:mm tt} for room {roomNumber}.");

        _logHandler.AddLog(new LogRecord(enterTime, day, reserverName, roomNumber, "Added"));
    }

    public void DeleteReservationByName(string reserverName, string roomNumber, string day, DateTime enterTime)
    {
        foreach (var dayReservations in weeklyReservations.Values)
        {
            foreach (var roomReservations in dayReservations.Values)
            {
                roomReservations.RemoveAll(reservation => reservation.Item2 == reserverName);
            }
        }

        _reservations.RemoveAll(r => r.ReserverName == reserverName && r.RoomNumber == roomNumber && r.Day == day); // Remove from the JSON list
        UpdateReservationDataFile(); // Update JSON file

        Console.WriteLine($"\nReservations made by {reserverName} have been removed.\n");

        _logHandler.AddLog(new LogRecord(enterTime, day, reserverName, roomNumber, "Deleted"));
    }
}
