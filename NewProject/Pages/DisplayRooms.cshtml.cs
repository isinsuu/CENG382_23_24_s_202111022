using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class DisplayRoomsModel : PageModel
    {
        private readonly AppDbContext _context;

        public DisplayRoomsModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Room> NewRoomList { get; set; } = new List<Room>();
        public void OnGet()
        {
            // Retrieve all rooms
            NewRoomList = _context.Rooms.ToList();
        }
    }
}
