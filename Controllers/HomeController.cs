using Microsoft.AspNetCore.Mvc;

namespace LoginPage.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Console.WriteLine("add");
            return View();
        }

        public IActionResult AddClient()
        {
            return RedirectToAction("Add", "Client"); // Redirect to Add action in ClientController
        }

        public IActionResult ViewClients()
        {
            return RedirectToAction("ViewClient", "Client"); // Redirect to View action in ClientController
        }
    }


}
