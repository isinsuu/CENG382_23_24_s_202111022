using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using loginDemo.Models;

namespace MyApp.Namespace
{
    [Authorize]
    public class CreateReservationModel : PageModel
    {
        private readonly WebAppDataBaseContext _context;
        private readonly ILogger<CreateReservationModel> _logger;

        public CreateReservationModel(WebAppDataBaseContext context, ILogger<CreateReservationModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public Reservation Reservation { get; set; } = new Reservation();
        public List<Room> Rooms { get; set; }

        public void OnGet()
        {
            Rooms = _context.Rooms.ToList();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Reservation.DateTime.Minute != 0 || Reservation.DateTime.Second != 0)
            {
                ModelState.AddModelError("Reservation.DateTime", "Please select a time at the top of the hour.");
                return Page();
            }

            Reservation.ReservedBy = User.Identity.Name;
            _context.Reservations.Add(Reservation);
            _context.SaveChanges();

            _logger.LogInformation("Reservation created by {UserName} for Room ID {RoomId} on {DateTime}", User.Identity.Name, Reservation.RoomId, Reservation.DateTime);

            return RedirectToPage("/DisplayReservation");
        }
    }
}
