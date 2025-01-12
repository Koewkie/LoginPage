using LoginPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;

namespace LoginPage.Controllers
{
    public class ClientController : Controller
    {
        string conn = "Data Source=localhost;Initial Catalog=CRM_Test_EstianHuman;Integrated Security=True;";
 

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
                return RedirectToAction("Error", "Client", new { e = ex.Message });
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

        [HttpPost]
        public IActionResult DeleteClient(int ID)
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    //Delete client login details
                    string queryDeleteLogin = @"DELETE FROM login_details WHERE client_id = @ClientId";
                    SqlCommand cmdDel = new SqlCommand(queryDeleteLogin, sqlcon);

                    cmdDel.Parameters.AddWithValue("@ClientId", ID);
                    cmdDel.ExecuteNonQuery();

                    //Delete client info
                    string queryDelete = @"DELETE FROM basic_client_information WHERE client_id = @ClientId";
                    cmdDel = new SqlCommand(queryDelete,sqlcon);

                    cmdDel.Parameters.AddWithValue("@ClientId", ID);
                    cmdDel.ExecuteNonQuery();
                    sqlcon.Close();
                }
                return RedirectToAction("ViewClient","Client");
            }
            catch(Exception ex) 
            {
                return RedirectToAction("Error", "Client", new { e = ex.Message });
            }
        }
    }
}