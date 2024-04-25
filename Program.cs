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
        Console.WriteLine($"Enterance Day: {enterDay}");
        Console.WriteLine($"Enterance Time: {enterHour}\n");
    }
}

class Program
{
    static void Main(string[] args)
    {
        string jsonFilePath = "Data.json";

        try
        {
            States state1 = new States("Isinsu", "001", "Tuesday", "10:20");
            States state2 = new States("Ceyda", "001", "Tuesday", "10:20");
            States state3 = new States("Sila", "002", "Tuesday", "10:20");
            States state4 = new States("Emre", "003", "Monday", "10:00");
            States selectedState = null;

            RoomHandler roomHandler = new RoomHandler(jsonFilePath);
            var roomData = roomHandler.GetRooms();

            ReservationHandler handler = new ReservationHandler(roomData);

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

                        Console.WriteLine("Select state 1:");
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
                        }


                        string reserverName = selectedState.name;
                        string roomNumber = selectedState.roomNumber;
                        string day = selectedState.enterDay;
                        DateTime time = DateTime.Parse(selectedState.enterHour);

                        handler.AddReservation(day, roomNumber, reserverName, time);
                        break;

                    case 2:
                        Console.Write("\nPlease provide the guest name to delete all reservations: ");
                        string reserverNameToDelete = Console.ReadLine();
                        handler.DeleteReservationByName(reserverNameToDelete);
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