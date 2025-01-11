using LoginPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;

namespace LoginPage.Controllers
{
    public class ClientController : Controller
    {
        string conn = "Data Source=localhost;Initial Catalog=CRM_Test_EstianHuman;Integrated Security=True;";
        // Action to show the Add Client view
        [HttpGet]
        public IActionResult Add()
        {
            List<TitleModel> titles = new List<TitleModel>();
            List<ClientTypeModel> clientTypes = new List<ClientTypeModel>();

            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    string query = @"SELECT * FROM title";
                    SqlCommand cmd = new SqlCommand(query, sqlcon);
                    SqlDataReader sr = cmd.ExecuteReader();

                    while (sr.Read())
                    {
                        var title = new TitleModel();

                        title.TitleId = sr.GetInt32(0);
                        title.TitleName = sr.GetString(1);

                        titles.Add(title);
                    }
                    sr.Close();

                    ViewBag.Titles = new SelectList(titles, "TitleId", "TitleName");
                    if (ViewBag.Titles == null)
                    {
                        return RedirectToAction("Error", new { e = "Titles is null" });
                    }
                }
            }
            catch (Exception ex)
            {
                // Pass nothing to the view
                return RedirectToAction("Error", new { e = ex.Message });
            }

            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn)) {
                    sqlcon.Open();
                    string query = @"SELECT * FROM client_type";
                    SqlCommand cmd = new SqlCommand(query, sqlcon);
                    SqlDataReader sr = cmd.ExecuteReader();

                    while (sr.Read())
                    {
                        var clientType = new ClientTypeModel();

                        clientType.ClientTypeId = sr.GetInt32(0);
                        clientType.ClientTypeName = sr.GetString(1);

                        clientTypes.Add(clientType);
                    }
                    sr.Close();
                    sqlcon.Close();

                    ViewBag.ClientTypes = new SelectList(clientTypes, "ClientTypeId", "ClientTypeName");
                    if (ViewBag.ClientTypes == null)
                    {
                        return RedirectToAction("Error", new { e = "ClientTypes is null" });
                    }
                }
            } catch (Exception ex)
            { 
                // Pass nothing to the view
                return RedirectToAction("Error", new { e = ex.Message });
            }

            return View(new ClientAddModel());
        }

        // Action to show the View Clients view
        [HttpGet]
        public IActionResult ViewClient()
        {
            //Get client data from db
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    string query = @"SELECT 
                                        bci.client_id, bci.name, bci.surname, t.title_name AS title,  
                                        ct.client_type_name as [Client Type], bci.email, bci.contact_number, bci.address 
                                    FROM 
                                        basic_client_information bci 
                                    INNER JOIN 
                                        client_type ct ON bci.client_type_id = ct.client_type_id 
                                    INNER JOIN
                                        title t ON bci.title_id = t.title_id";
                    SqlCommand cmd = new SqlCommand(query,sqlcon);
                    SqlDataReader sr = cmd.ExecuteReader();

                    var clients = new List<ClientViewModel>();

                    while (sr.Read())
                    {
                        var client = new ClientViewModel();

                        client.ClientId = sr.GetInt32(0);
                        client.Name = sr.GetString(1);
                        client.Surname = sr.GetString(2);
                        client.Title = sr.GetString(3);
                        client.ClientType = sr.GetString(4);
                        client.Email = sr.GetString(5);
                        client.ContactNumber = sr.GetString(6);
                        client.Address = sr.GetString(7);

                        clients.Add(client);
                    }

                    sqlcon.Close();
                    if (clients.Count == 0) //if client table is empty
                    {
                        // Pass nothing to the view
                        return View();
                    }

                    // Pass the data to the view
                    return View(clients);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Client view database exception: "+ex.Message);
                // Pass nothing to the view
                return View();
            } 
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error(string e)
        {
            ViewData["ErrorMessage"] = e;
            return View();
        }
    }
}