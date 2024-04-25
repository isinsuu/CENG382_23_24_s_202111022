public interface IReservationService
{
    void AddReservation(string day, string roomNumber, string reserverName, DateTime enterTime);
    void DeleteReservation(string reserverName);
    void DisplayWeeklySchedule();
}
