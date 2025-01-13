using LoginPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Json;

namespace LoginPage.Controllers
{
    public class ClientController : Controller
    {
        string conn = "Data Source=localhost;Initial Catalog=CRM_Test_EstianHuman;Integrated Security=True;";
        private readonly HttpClient _client;

        public ClientController(HttpClient client)
        {
            _client = client;
        }

        //Action to get clients from api and show
        public async Task<IActionResult> ViewClient()
        {
            try
            {
                // Call the api get all clients
                var response = await _client.GetFromJsonAsync<List<ClientViewModel>>("http://localhost:5035/api/client/GetAllClients");

                if (response == null || response.Count == 0) // if none returned
                {
                    return View();
                }

                // Pass the data to the view
                return View(response);
            }
            catch (Exception ex)
            {
                //Error view
                return RedirectToAction("Error", "Client", new { e = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Error(string e)
        {
            //Goes to error view
            ViewData["ErrorMessage"] = e;
            return View();
        }


        public async Task<IActionResult> DeleteClient(int id)
        {
            try
            {
                // Call the api get all clients
                var response = await _client.DeleteAsync($"http://localhost:5035/api/client/DeleteClient/{id}");

                if (response.IsSuccessStatusCode) // if delete successful
                {
                    return RedirectToAction("ViewClient", "Client"); //go back to view clients
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync(); //else get error message and go to error view
                    return RedirectToAction("Error", "Client", new { e = errorMessage });
                }
            }
            catch (Exception ex)
            {
                //Error view
                return RedirectToAction("Error", "Client", new { e = ex.Message });
            }
        }
    }
}