using System.Text.Json;
using System.Text.Json.Serialization;

public record Room
{
    [JsonPropertyName("roomId")]
    public string ?RoomId { get; set; }

    [JsonPropertyName("roomName")]
    public string ?RoomName { get; set; }

    [JsonPropertyName("capacity")]
    public int Capacity { get; set; }
}