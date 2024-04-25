public class LogHandler
{
    private readonly ILogger logger;
    
    public LogHandler(ILogger logger) // Dependency Injection principle
    {
        this.logger = logger;
    }

    public void AddLog(LogRecord log)
    {
        logger.Log(log);
    }
}