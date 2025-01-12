using LoginPage.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoginPage.Controllers
{
    public class NotesController : Controller
    {
        public IActionResult Notes()
        {

            var notes = new List<NotesModel>
        {
            new NotesModel
            {
                ClientID = 1,
                NoteID = 1,
                EmployeeComment = "\"Lorem ipsum dolor sit amet, consectetur adipiscing elit. " +
                "Integer nec odio. Praesent libero. Sed cursus ante dapibus diam. Sed nisi. " +
                "Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum. Praesent mauris. " +
                "Fusce nec tellus sed augue semper porta. Mauris massa.\"",
                ClientComment = "\"Lorem ipsum dolor sit amet, consectetur adipiscing elit. " +
                "Integer nec odio. Praesent libero. Sed cursus ante dapibus diam. Sed nisi. " +
                "Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum. Praesent mauris. " +
                "Fusce nec tellus sed augue semper porta. Mauris massa.\"",
                Date = DateTime.Now.AddDays(-2)
            },
            new NotesModel
            {
                ClientID = 1,
                NoteID = 2,               
                EmployeeComment = "Followed up on the issue.",
                ClientComment = "Still waiting for resolution.",
                Date = DateTime.Now.AddDays(-1)
            },
            new NotesModel
            {
                ClientID = 1,
                NoteID = 3,                
                EmployeeComment = "Provided a discount for loyalty.",
                ClientComment = "Thank you for the discount!",
                Date = DateTime.Now
            }
        };

            return View(notes);
        }
        
        public IActionResult AddEmployeeComment()
        {
            return View();
        }
        public IActionResult EditComment()
        {
            return View();
        }
    }
}
