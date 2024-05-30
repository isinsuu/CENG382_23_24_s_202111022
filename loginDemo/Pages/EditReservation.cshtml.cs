using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using loginDemo.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyApp.Namespace
{
    public class EditReservationModel : PageModel
    {
        private readonly WebAppDataBaseContext _context;

        public EditReservationModel(WebAppDataBaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Reservation Reservation { get; set; }

        public SelectList Rooms { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Reservation = await _context.Reservations
                                         .Include(r => r.Room)
                                         .FirstOrDefaultAsync(r => r.Id == id);

            if (Reservation == null)
            {
                return NotFound();
            }

            var rooms = await _context.Rooms.ToListAsync();
            Rooms = new SelectList(rooms, "Id", "RoomName");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var rooms = await _context.Rooms.ToListAsync();
                Rooms = new SelectList(rooms, "Id", "RoomName");
                return Page();
            }

            var reservationToUpdate = await _context.Reservations.FindAsync(Reservation.Id);

            if (reservationToUpdate == null)
            {
                return NotFound();
            }

            bool isConflict = _context.Reservations
                .Any(r => r.DateTime == Reservation.DateTime &&
                          r.RoomId == Reservation.RoomId &&
                          !r.IsDeleted);

            if (isConflict)
            {
                ModelState.AddModelError("Reservation.DateTime", "A reservation for this room at the same date and time already exists.");
                return Page();
            }

            reservationToUpdate.RoomId = Reservation.RoomId;
            reservationToUpdate.DateTime = Reservation.DateTime;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(Reservation.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./DisplayReservation");
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
