public interface IReservationService
{
    void AddReservation(string day, string roomNumber, string reserverName, DateTime enterTime);
    void DeleteReservation(string reserverName);
    void PrintWeeklySchedule();
    void InitializeReservations(string jsonFilePath);
    List<Reservation> GetReservationsByReserverName(string name);
    List<Reservation> GetAllReservations();
}
