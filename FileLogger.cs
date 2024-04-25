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
    }

    public void Log(LogRecord log)
    {
        var logs = new List<LogRecord>();

        if (File.Exists(logFilePath))
        {
            var jsonString = File.ReadAllText(logFilePath);
            logs = JsonSerializer.Deserialize<List<LogRecord>>(jsonString) ?? new List<LogRecord>();
        }

        logs.Add(log);

        var updatedJsonString = JsonSerializer.Serialize(logs, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(logFilePath, updatedJsonString);
    }
}
