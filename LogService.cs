using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

public class LogEntry
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("dayName")]
    public string DayName { get; set; }

    [JsonPropertyName("ReserverName")]
    public string ReserverName { get; set; }

    [JsonPropertyName("RoomName")]
    public string RoomName { get; set; }

    [JsonPropertyName("action")]
    public string Action { get; set; }
}

public class LogService
{
    private static List<LogEntry> _logs = new List<LogEntry>();
    public static List<Reservation> logs = new List<Reservation>();
    private static string _logFilePath = "LogData.json"; 

    public static void InitializeLogs()
    {
        if (!File.Exists(_logFilePath))
        {
            Console.WriteLine("Log file not found, initializing an empty log list.");
            _logs = new List<LogEntry>();
            return;
        }

        try
        {
            string json = File.ReadAllText(_logFilePath);
            _logs = JsonSerializer.Deserialize<List<LogEntry>>(json) ?? new List<LogEntry>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to initialize logs: {ex.Message}");
            _logs = new List<LogEntry>();  
        }
    }

    public static List<LogEntry> DisplayLogsByName(string name)
    {
        if (_logs == null)
        {
            Console.WriteLine("Log data is not loaded.");
            return new List<LogEntry>();  
        }

        return _logs.Where(log => string.Equals(log.ReserverName, name, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public static void AddLogEntry(LogEntry logEntry)
    {
        _logs.Add(logEntry);
        UpdateLogFile();
    }

    private static void UpdateLogFile()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(_logs, options);
        File.WriteAllText(_logFilePath, json);
    }
    public static List<LogEntry> DisplayLogs(DateTime start)
    {
        DateTime end = start.AddMinutes(40); // End time is 40 minutes after the start
        return _logs.Where(log => log.Timestamp >= start && log.Timestamp <= end).ToList();
    }
}