using Microsoft.AspNetCore.Mvc;

namespace LoginPage.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RedirectToAddClient()
        {
            return RedirectToAction("Add", "Client"); // Redirect to Add action in ClientController
        }

        public IActionResult RedirectToViewClients()
        {
            return RedirectToAction("View", "Client"); // Redirect to View action in ClientController
        }
    }


}
