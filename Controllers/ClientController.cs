using Microsoft.AspNetCore.Mvc;

namespace LoginPage.Controllers
{
    public class ClientController : Controller
    {
        // Action to show the Add Client view
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // Action to show the View Clients view
        [HttpGet]
        public IActionResult ViewClient()
        {
            // Placeholder: Replace with logic to fetch clients from a database
            var clients = new List<string> { "Client 1", "Client 2", "Client 3" };
            return View(clients);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }
    }
}