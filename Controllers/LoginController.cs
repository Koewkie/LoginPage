using Microsoft.AspNetCore.Mvc;

namespace LoginPage.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("ViewClient", "Client");    //fisrt index->IActionResult Index in homecontroller
                                                             //second home->Home part of the HomeController.cs
            }
            else
            {
                return View();
            }
        }
    }
}
