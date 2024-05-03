using System.Text.Json;
using System.Text.Json.Serialization;

/*My program from last lab violated the Single Responsibility Principle and Dependency Injection Principle
due to a few higher level classes being dependent on lower level classes and handling too many responsibilites.*/

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

        ILogger fileLogger = new FileLogger(logFilePath);
        LogHandler logHandler= new LogHandler(fileLogger);
        IReservationRepository reservationRepository = new ReservationRepository();
        RoomHandler roomHandler = new RoomHandler(jsonFilePath);

        try
        {
            /*States state1 = new States("Isinsu", "001", "Tuesday", "10:20");
            States state2 = new States("Ceyda", "001", "Tuesday", "10:20");
            States state3 = new States("Sila", "002", "Tuesday", "10:20");
            States state4 = new States("Emre", "003", "Monday", "10:00");
            States selectedState = null;*/

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
                Console.WriteLine("To exit press 4.");

                int selection = int.Parse(Console.ReadLine());

                switch (selection)
                {
                    case 1:

                        /*Console.WriteLine("Select state 1:");
                        state1.displayProperty();
                        Console.WriteLine("Select state 2:");
                        state2.displayProperty();
                        Console.WriteLine("Select state 3:");
                        state3.displayProperty();
                        Console.WriteLine("Select state 4:");
                        state4.displayProperty();

                        int selection2 = int.Parse(Console.ReadLine());

                        switch (selection2)
                        {
                            case 1:
                                selectedState = state1;
                                break;
                            case 2:
                                selectedState = state2;
                                break;
                            case 3:
                                selectedState = state3;
                                break;
                            case 4:
                                selectedState = state4;
                                break;
                            default:
                                Console.WriteLine("Invalid input.");
                                break;
                        }*/

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
                        handler.PrintWeeklySchedule();
                        break;

                    case 4:
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
}