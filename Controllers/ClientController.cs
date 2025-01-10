using LoginPage.Models;
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
            // Sample data
            var clients = new List<ClientViewModel>
            {
                new ClientViewModel { ClientId = 1, Name = "John", Surname = "Doe", Title = "Mr.", ClientType = "New Client", Email = "john.doe@example.com", ContactNumber = "123-456-7890", Address = "123 Main Street, Cityville" },
                new ClientViewModel { ClientId = 2, Name = "Jane", Surname = "Smith", Title = "Ms.", ClientType = "Important Client", Email = "jane.smith@example.com", ContactNumber = "234-567-8901", Address = "456 Elm Street, Townsville" },
                new ClientViewModel { ClientId = 3, Name = "Michael", Surname = "Brown", Title = "Dr.", ClientType = "Super Client", Email = "michael.brown@example.com", ContactNumber = "345-678-9012", Address = "789 Oak Avenue, Villageton" },
                new ClientViewModel { ClientId = 4, Name = "Emily", Surname = "Johnson", Title = "Mrs.", ClientType = "Client Removed", Email = "emily.johnson@example.com", ContactNumber = "456-789-0123", Address = "321 Pine Road, Hamletburg" }
            };

            // Pass the data to the view
            return View(clients);
        }
        //public IActionResult ViewClient()
        //{
        //    // Placeholder: Replace with logic to fetch clients from a database
        //    var clients = new List<string> { "Client 1", "Client 2", "Client 3" };
        //    return View(clients);
        //}
    }
}