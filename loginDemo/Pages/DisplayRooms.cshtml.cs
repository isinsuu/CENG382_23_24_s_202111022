using loginDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class DisplayRoomsModel : PageModel
    {
        private readonly WebAppDataBaseContext _context;

        public DisplayRoomsModel(WebAppDataBaseContext context)
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