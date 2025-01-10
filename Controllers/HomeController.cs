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
    }


}
