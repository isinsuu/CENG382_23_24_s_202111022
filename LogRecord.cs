public record LogRecord(
    DateTime Timestamp,
    string ReserverName,
    string RoomName,
    string Action // "Added" or "Deleted" for reservations
);
