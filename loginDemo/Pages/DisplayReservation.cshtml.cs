using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using loginDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Namespace
{
    [Authorize]
    public class DisplayReservationModel : PageModel
    {
        private readonly WebAppDataBaseContext _context;

        public DisplayReservationModel(WebAppDataBaseContext context)
        {
            _context = context;
        }

        public DateTime startOfWeek { get; set; }
        public IList<Reservation> ReservationsList { get; set; } = new List<Reservation>();

        public void OnGet(string roomName, DateTime? startDate, int? capacity)
        {
            var today = DateTime.Today;
            int currentDayOfWeek = (int)today.DayOfWeek;
            int daysUntilMonday = (currentDayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            startOfWeek = today.AddDays(-daysUntilMonday);

            DateTime endOfWeek = startOfWeek.AddDays(7);

            var query = _context.Reservations
                                .Include(r => r.Room)
                                .Where(r => !r.IsDeleted) 
                                .AsQueryable(); 

            if (!string.IsNullOrEmpty(roomName))
            {
                query = query.Where(r => r.Room.RoomName == roomName);
            }

            if (startDate.HasValue)
            {
                var endDate = startDate.Value.AddDays(1); 
                query = query.Where(r => r.DateTime >= startDate.Value && r.DateTime < endDate);
            }

            if (capacity.HasValue)
            {
                query = query.Where(r => r.Room.Capacity >= capacity);
            }

            ReservationsList = query.ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int reservationId)
        {
            var reservation = await _context.Reservations.FindAsync(reservationId);
            if (reservation == null)
            {
                return NotFound();
            }

            reservation.IsDeleted = true;
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

    }
}
