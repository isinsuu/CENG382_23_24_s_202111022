using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

public record RoomData
{
    [JsonPropertyName("Room")]
    public Room[] Rooms { get; set; }
}

public class RoomHandler
{
    private string jsonFilePath;

    public RoomHandler(string filePath)
    {
        jsonFilePath = filePath;
    }

    public RoomData GetRooms()
    {
        RoomData roomData = null;

        try
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            var options = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
            };
            roomData = JsonSerializer.Deserialize<RoomData>(jsonString, options);
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

        return roomData;
    }

    public void SaveRooms(List<Room> rooms)
    {
        try
        {
            RoomData roomData = new RoomData { Rooms = rooms.ToArray() };
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(roomData, options);
            File.WriteAllText(jsonFilePath, jsonString);
            Console.WriteLine("Rooms data saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving rooms data: {ex.Message}");
        }
    }
}