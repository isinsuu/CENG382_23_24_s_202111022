using loginDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{   
    [Authorize]
    public class CreateRoomsModel : PageModel
    {
        private readonly WebAppDataBaseContext _context;

        public CreateRoomsModel(WebAppDataBaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Room NewRoom { get; set; } = new Room(); 
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || NewRoom == null)
            {
                return Page();
            }

            if (_context.Rooms.Any(r => r.RoomName == NewRoom.RoomName))
            {
                ModelState.AddModelError("NewRoom.RoomName", "A room with this name already exists. Enter a unique name.");
                return Page();
            }

            _context.Rooms.Add(NewRoom);
            _context.SaveChanges();

            var log = new LogInformation{
                UserId = User.Identity.Name,
                RoomId = NewRoom.Id,
                Timestamp = DateTime.Now,
            };

            _context.LogInformations.Add(log);
            _context.SaveChanges();

            return RedirectToPage("/DisplayRooms");
        }
    }
}