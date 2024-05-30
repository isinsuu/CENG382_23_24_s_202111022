namespace loginDemo.Models;

public partial class Reservation
{
    public int Id { get; set; }

    public int? RoomId { get; set; }

    public DateTime DateTime { get; set; }

    public string? ReservedBy { get; set; }

    public virtual Room? Room { get; set; }

    public bool IsDeleted { get; set; }
}
