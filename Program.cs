using System.Text.Json;
using System.Text.Json.Serialization;

// States for you to try
public class States
{
    public string name { get; set; }
    public string roomNumber { get; set; }
    public string enterDay { get; set; }
    public string enterHour { get; set; }

    public States(string Name, string RoomNumber, string Enter, string Exit)
    {
        name = Name;
        roomNumber = RoomNumber;
        enterDay = Enter;
        enterHour = Exit;
    }
    public void displayProperty()
    {
        Console.WriteLine($"Name: {name}");
        Console.WriteLine($"Room Number: {roomNumber}");
        Console.WriteLine($"Entrance Day: {enterDay}");
        Console.WriteLine($"Entrance Time: {enterHour}\n");
    }
}

class Program
{
    static void Main(string[] args)
    {
        string jsonFilePath = "Data.json";
        string logFilePath = "LogData.Json";
        string reservationDataPath = "ReservationData.json";

        ILogger fileLogger = new FileLogger(logFilePath);
        LogHandler logHandler= new LogHandler(fileLogger);
        IReservationRepository reservationRepository = new ReservationRepository();
        RoomHandler roomHandler = new RoomHandler(jsonFilePath);
        LogService.InitializeLogs();

        try
        {
            Dictionary<string, States> statesDictionary = new Dictionary<string, States>
            {
                {"Isinsu", new States("Isinsu", "001", "Tuesday", "10:20")},
                {"Ceyda", new States("Ceyda", "001", "Tuesday", "10:20")},
                {"Sila", new States("Sila", "002", "Tuesday", "10:20")},
                {"Emre", new States("Emre", "003", "Monday", "10:00")}
            };

            var roomData = roomHandler.GetRooms();
            ReservationHandler handler = new ReservationHandler(roomData, reservationRepository, logHandler);
            ReservationService reservationService= new ReservationService(handler, reservationRepository);

            bool programOn = true;
            while (programOn)
            {
                Console.WriteLine("To add a new reservation press 1.");
                Console.WriteLine("To delete reservation press 2.");
                Console.WriteLine("To display weekly schedule press 3.");
                Console.WriteLine("To search for reservations by entering reserver name press 4.");
                Console.WriteLine("To search for reservations by entering roomID press 5.");
                Console.WriteLine("To display all logs by entering reserver name press 6.");
                Console.WriteLine("To exit press 7.");

                int selection = int.Parse(Console.ReadLine());

                switch (selection)
                {
                    case 1:
                        int index = 1;
                        foreach (var state in statesDictionary)
                        {
                            Console.WriteLine($"Select state {index}:");
                            state.Value.displayProperty();
                            index++;
                        }

                        Console.WriteLine("Enter the number of the state to select:");
                        if (int.TryParse(Console.ReadLine(), out int stateSelection) && stateSelection > 0 && stateSelection <= statesDictionary.Count)
                        {
                            // Convert state selection from 1-based to 0-based index by subtracting 1 and then accessing the element.
                            States selectedState = statesDictionary.ElementAt(stateSelection - 1).Value;

                            string reserverName = selectedState.name;
                            string roomNumber = selectedState.roomNumber;
                            string day = selectedState.enterDay;
                            DateTime time = DateTime.Parse(selectedState.enterHour);

                            handler.AddReservation(day, roomNumber, reserverName, time);
                        }

                        break;

                    case 2:
                        Console.Write("\nPlease provide the guest name to delete all reservations: ");
                        string reserverNameToDelete = Console.ReadLine();

                        if (statesDictionary.TryGetValue(reserverNameToDelete, out States providedState))
                        {   
                            string reserverName = providedState.name;
                            string roomNumber = providedState.roomNumber;
                            string day = providedState.enterDay;
                            DateTime time = DateTime.Parse(providedState.enterHour);
                            handler.DeleteReservationByName(reserverNameToDelete, roomNumber, day, time);
                        }

                        break;

                    case 3:
                        ReservationService.InitializeReservations("ReservationData.json");
                        reservationService.PrintWeeklySchedule();
                        break;

                    case 4:
                        ReservationService.InitializeReservations("ReservationData.json");
                        SearchReservationsByName(reservationService);
                        break;

                    case 5:
                        Console.WriteLine("Enter the Room ID: ");
                        string roomId = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(roomId))
                        {
                            Console.WriteLine("Room ID cannot be empty!");
                            return;
                        }
                        SearchReservationsByRoomId(roomId, reservationService);
                        break;

                    case 6:   
                        LogService.InitializeLogs();
                        Console.WriteLine("Enter the name of the reserver to display logs:");
                        string reserver = Console.ReadLine();
                        var logs = LogService.DisplayLogsByName(reserver);
                        if (logs.Any())
                        {
                            foreach (var log in logs)
                            {
                                Console.WriteLine($"Timestamp: {log.Timestamp}, Room: {log.RoomName}, Action: {log.Action}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No logs found for this reserver.");
                        }
                        break;

                    case 7:
                        programOn = false;
                        Console.WriteLine("\nI hope you enjoyed our service!");
                        break;

                    default:
                        Console.WriteLine("Invalid input, please try again.");
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

    static void SearchReservationsByName(ReservationService reservationService)
    {
        Console.WriteLine("Enter Reserver Name:");
        string reserverName = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(reserverName))
        {
            Console.WriteLine("Reserver name cannot be empty!");
            return;
        }

        try
        {
            var reservations = reservationService.GetReservationsByReserverName(reserverName);
            if (reservations.Any())
            {
                foreach (var reservation in reservations)
                {
                    Console.WriteLine($"Reservation for {reservation.ReserverName} in room {reservation.RoomNumber} on {reservation.Day} at {reservation.EnterTime}");
                }
            }
            else
            {
                Console.WriteLine("No reservations found for this reserver.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public static void SearchReservationsByRoomId(string roomId, ReservationService reservationService)
    {
        var allReservations = reservationService.GetAllReservations(); 

        if (allReservations == null)
        {
            Console.WriteLine("Reservation data is not available.");
            return;
        }

        var filteredReservations = allReservations.Where(r => r.RoomNumber == roomId).ToList();

        if (filteredReservations.Any())
        {
            foreach (var reservation in filteredReservations)
            {
                Console.WriteLine($"Reservation for {reservation.ReserverName} in room {reservation.RoomNumber} on {reservation.Day} at {reservation.EnterTime}");
            }
        }
        else
        {
            Console.WriteLine("No reservations found for this room.");
        }
    }
}