namespace loginDemo.Models;
public class LogInformation
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public int? ReservationId { get; set; }
    public int? RoomId {get; set; }
    public DateTime Timestamp { get; set; }
    public DateTime? StartEndDate { get; set; }
    public virtual Reservation? Reservation { get; set; }
    public virtual Room? Room { get; set; }
}