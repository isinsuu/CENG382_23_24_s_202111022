using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class CreateRoomsModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateRoomsModel(AppDbContext context)
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

            _context.Rooms.Add(NewRoom);
            _context.SaveChanges();
            return RedirectToPage("/DisplayRooms");
        }
    }
}
