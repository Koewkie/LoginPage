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
                EmployeeComment = "Suggested a new design layout.",
                ClientComment = "We like the suggestion!",
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
