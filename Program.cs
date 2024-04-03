using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

public class RoomData
{
    [JsonPropertyName("Room")]
    public Room[] Rooms { get; set; }
}

public class Room
{
    [JsonPropertyName("roomId")]
    public string RoomId { get; set; }

    [JsonPropertyName("roomName")]
    public string RoomName { get; set; }

    [JsonPropertyName("capacity")]
    public int Capacity { get; set; }
}

public class Reservation
{
    public Room Room { get; set; }
    public DateTime Date { get; set; }
    public DateTime Time { get; set; }
    public string ReserverName { get; set; }
}


public class ReservationHandler
{
    private Reservation[,] reservations;
    private RoomData roomData;

    public ReservationHandler(int days, int numberOfRooms, RoomData roomData_sent)
    {
        reservations = new Reservation[numberOfRooms, days];
        roomData = roomData_sent;
    }

    public void AddReservation(Reservation reservation)
    {
        int startIndex = reservation.Date.Day - 1;
        int numberOfDays = reservation.Time.Day - startIndex;
        int roomIndex = int.Parse(reservation.Room.RoomId) - 1;

        if (startIndex < 0 || startIndex + numberOfDays > reservations.GetLength(1))
        {
            Console.WriteLine("Reservation exceeds the month's timeframe.");
            return;
        }

        for (int i = startIndex; i < startIndex + numberOfDays; i++)
        {
            if (reservations[roomIndex, i] != null)
            {
                Console.WriteLine($"Reservation overlap detected on {reservation.Date.AddDays(i - startIndex):MM/dd/yyyy}");
                return;
            }
        }

        for (int i = startIndex; i < startIndex + numberOfDays; i++)
        {
            reservations[roomIndex, i] = reservation;
        }

        Console.WriteLine($"Reservation added from {reservation.Date:dd/MM/yyyy} to {reservation.Time:dd/MM/yyyy} for room {reservation.Room.RoomName}.");
    }

    public void DeleteReservation(string reserverName)
    {
        for (int i = 0; i < reservations.GetLength(0); i++)
        {
            for (int j = 0; j < reservations.GetLength(1); j++)
            {
                if (reservations[i, j]?.ReserverName == reserverName)
                {
                    reservations[i, j] = null;
                }
            }
        }

        Console.WriteLine($"\nReservations made by {reserverName} have been removed.\n");
    }

    public void DisplayWeeklySchedule()
    {
        Console.WriteLine("Weekly Schedule of Reservations:");
        for (int i = 0; i < reservations.GetLength(0); i++)
        {
            Console.WriteLine($"Room {i + 1} - {roomData.Rooms[i].RoomName}:");
            for (int j = 0; j < reservations.GetLength(1); j++)
            {
                if (reservations[i, j] != null && reservations[i, j] != reservations[i, j + 1])
                {
                    Console.WriteLine($"  {reservations[i, j].Date:dd/MM/yyyy} to {reservations[i, j].Time:dd/MM/yyyy}: {reservations[i, j].ReserverName}");
                }
            }
            Console.WriteLine();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        string jsonFilePath = "Data.json";

        try
        {
            string jsonString = File.ReadAllText(jsonFilePath);

            var options = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
            };

            var roomData = JsonSerializer.Deserialize<RoomData>(jsonString, options);

            ReservationHandler handler = new ReservationHandler(28, roomData.Rooms.Length, roomData);

            bool programOn = true;
            while (programOn)
            {
                Console.WriteLine("To add a new reservation press 1.");
                Console.WriteLine("To delete reservation press 2.");
                Console.WriteLine("To display weekly schedule press 3.");
                Console.WriteLine("To exit press 4.");

                int selection = int.Parse(Console.ReadLine());

                switch (selection)
                {
                    case 1:

                        Console.WriteLine("\nAll the Rooms:");
                        for (int i = 0; i < roomData.Rooms.Length; i++)
                        {
                            Console.WriteLine($"{i + 1}. {roomData.Rooms[i].RoomName}");
                        }

                        Console.Write("Enter reserver name: ");
                        string reserverName = Console.ReadLine();

                        Console.Write("Select the room you want to reserve (Enter room number): ");
                        int roomIndex = int.Parse(Console.ReadLine()) - 1;

                        if (roomIndex < 0 || roomIndex >= roomData.Rooms.Length)
                        {
                            Console.WriteLine("Invalid room selection. Such room does not exist.");
                            break;
                        }

                        Console.Write("Enter the entrance day for your reservation (DD/MM/YYYY): ");
                        DateTime date = DateTime.Parse(Console.ReadLine());

                        Console.Write("Enter the exit day for your reservation (DD/MM/YYYY): ");
                        DateTime time = DateTime.Parse(Console.ReadLine());

                        Reservation newReservation = new Reservation
                        {
                            Date = date,
                            Time = time,
                            ReserverName = reserverName,
                            Room = roomData.Rooms[roomIndex]
                        };

                        handler.AddReservation(newReservation);

                        break;

                    case 2:
                        Console.Write("\nPlease provide the guest name to delete all reservations: ");
                        string reserverNameToDelete = Console.ReadLine();
                        handler.DeleteReservation(reserverNameToDelete);
                        
                        break;

                    case 3:
                        handler.DisplayWeeklySchedule();
                        break;

                    case 4:
                        programOn = false;
                        Console.WriteLine("I hope you enjoyed our service!");
                        break;
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"File '{jsonFilePath}' not found.");
        }
        catch (JsonException)
        {
            Console.WriteLine($"Error deserializing JSON data from file '{jsonFilePath}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
