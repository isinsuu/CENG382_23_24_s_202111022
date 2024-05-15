using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class FileLogger : ILogger
{
    private readonly string logFilePath;

    public FileLogger(string logFilePath)
    {
        this.logFilePath = logFilePath;

        if (!File.Exists(logFilePath) || File.ReadAllText(logFilePath).Trim() == "")
        {
            File.WriteAllText(logFilePath, JsonSerializer.Serialize(new List<LogRecord>(), new JsonSerializerOptions { WriteIndented = true }));
        }
    }

    public void Log(LogRecord log)
    {
        var logs = new List<LogRecord>();

        try
        {
            var jsonString = File.ReadAllText(logFilePath);
            logs = JsonSerializer.Deserialize<List<LogRecord>>(jsonString) ?? new List<LogRecord>();
        }
        catch (JsonException ex)
        {
            Console.WriteLine("Failed to deserialize the log file. Error: " + ex.Message);
            logs = new List<LogRecord>();
        }

        logs.Add(log);

        var updatedJsonString = JsonSerializer.Serialize(logs, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(logFilePath, updatedJsonString);
    }
}


