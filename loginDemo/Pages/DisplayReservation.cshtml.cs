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

        public IList<Reservation> ReservationsList { get; set; } = new List<Reservation>();

        public void OnGet(string roomName, DateTime? startDate, int? capacity)
        {
            DateTime startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            DateTime endOfWeek = startOfWeek.AddDays(7);

            var query = _context.Reservations.Include(r => r.Room).AsQueryable(); 

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

        public IActionResult OnPostDelete(int reservationId)
        {
            var reservation = _context.Reservations.Find(reservationId);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            _context.SaveChanges();

            return RedirectToPage();
        }

    }
}
