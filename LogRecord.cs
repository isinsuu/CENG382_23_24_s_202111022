public class LogRecord{
    public DateTime timestamp { get; set; }
    public string dayName { get; set; }
    public string ReserverName { get; set; }
    public string RoomName { get; set; }
    public string action { get; set; }// "Added" or "Deleted"

    public LogRecord(){}

    public LogRecord(DateTime timestamp, string dayName, string ReserverName, string RoomName, string action){
        this.timestamp = timestamp;
        this.dayName = dayName;
        this.ReserverName = ReserverName;
        this.RoomName = RoomName;
        this.action = action;
    }
}
