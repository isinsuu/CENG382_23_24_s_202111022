using System.Security.Cryptography;

public class LogHandler
{
    private readonly ILogger _logger;
    
    public LogHandler(ILogger logger) // Dependency Injection principle
    {
        _logger = logger;
    }

    public void AddLog(LogRecord log)
    {
        _logger.Log(log);
    }
}