using LoginPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using System.Reflection;

namespace LoginPage.Controllers
{
    public class EditController : Controller
    {
        string conn = "Data Source=localhost;Initial Catalog=CRM_Test_EstianHuman;Integrated Security=True;";
        // Action to show the Edit Client 
        [HttpGet]
        public IActionResult Edit(int ID, ClientEditModel model = null)
        {
            List<TitleModel> titles = new List<TitleModel>();
            List<ClientTypeModel> clientTypes = new List<ClientTypeModel>();
            if (model == null)
            {
                model = new ClientEditModel();
            }
            //Get titles from DB for viewbag
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
            //Get ClientTypes from DB for viewbag
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
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
            }
            catch (Exception ex)
            {
                // Pass nothing to the view
                return RedirectToAction("Error", new { e = ex.Message });
            }

            //Get existing Client Info from DB for fields
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();

                    string query = @"SELECT 
                                        bci.title_id, bci.client_type_id, bci.name, bci.surname, bci.email, 
                                        bci.contact_number, bci.address, ld.username 
                                    FROM 
                                        basic_client_information AS bci
                                    INNER JOIN
                                        login_details AS ld ON bci.client_id = ld.client_id
                                    WHERE 
                                        bci.client_id = @ClientId";

                    SqlCommand cmd = new SqlCommand(query, sqlcon);
                    cmd.Parameters.AddWithValue("@ClientId", ID);

                    SqlDataReader sr = cmd.ExecuteReader();

                    if (sr.Read())
                    {
                        model.ClientId = ID;
                        model.TitleId = sr.GetInt32(0);
                        model.ClientTypeId = sr.GetInt32(1);
                        model.Name = sr.GetString(2);
                        model.Surname = sr.GetString(3);
                        model.Email = sr.GetString(4);
                        model.ContactNumber = sr.GetString(5);
                        model.Address = sr.GetString(6);
                        model.Username = sr.GetString(7);
                    }
                    sr.Close();
                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                // Pass nothing to the view
                return RedirectToAction("Error", new { e = ex.Message });
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult EditClient(ClientEditModel model)
        {
            if (model.ProfilePicture == null)
            {
                ModelState.Remove("ProfilePicture");
            }
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please ensure all fields are correctly filled in";

                return RedirectToAction("Edit", model);
            }

            try
            {
                // Convert profile picture to byte array
                byte[] profilePictureData = null;
                if (model.ProfilePicture != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        model.ProfilePicture.CopyTo(memoryStream);
                        profilePictureData = memoryStream.ToArray();
                    }
                }

                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    //query string for checking if username already exists
                    string userNameQuery = @"SELECT Count(*) FROM login_details 
                                             WHERE username = @UserName AND client_id != @ClientId";

                    using (SqlCommand chkUserName = new SqlCommand(userNameQuery, sqlcon))
                    {
                        chkUserName.Parameters.AddWithValue("@UserName", model.Username);
                        chkUserName.Parameters.AddWithValue("@ClientId", model.ClientId);

                        if ((int)chkUserName.ExecuteScalar() > 0)
                        {
                            TempData["ErrorMessage"] = "The username already exists.";
                            var newModel = new ClientAddModel();
                            newModel.Name = model.Name;


                            return RedirectToAction("Edit", model);
                        }
                    }

                    if (model.ProfilePicture != null)
                    {
                        //query string for updating client (with profile picture)
                        string queryUpdateClient = @"UPDATE basic_client_information                                            
                                         SET
                                            title_id = @TitleId,
                                            client_type_id = @ClientTypeId,
                                            name = @Name,
                                            surname = @Surname,
                                            email = @Email,
                                            contact_number = @ContactNumber,
                                            address = @Address,
                                            profile_picture = @ProfilePicture
                                        WHERE
                                            client_id = @ClientId";

                        //adding parameters with model data
                        using (SqlCommand updtCmd = new SqlCommand(queryUpdateClient, sqlcon))
                        {
                            updtCmd.Parameters.AddWithValue("@ClientId", model.ClientId);
                            updtCmd.Parameters.AddWithValue("@ProfilePicture", profilePictureData);
                            updtCmd.Parameters.AddWithValue("@TitleId", model.TitleId);
                            updtCmd.Parameters.AddWithValue("@ClientTypeId", model.ClientTypeId);
                            updtCmd.Parameters.AddWithValue("@Name", model.Name);
                            updtCmd.Parameters.AddWithValue("@Surname", model.Surname);
                            updtCmd.Parameters.AddWithValue("@Email", model.Email);
                            updtCmd.Parameters.AddWithValue("@ContactNumber", model.ContactNumber);
                            updtCmd.Parameters.AddWithValue("@Address", model.Address);
                            updtCmd.ExecuteNonQuery();
                        }
                    }else
                    {
                        //query string for updating client (without profilepicture)
                        string queryUpdateClient = @"UPDATE basic_client_information                                            
                                         SET
                                            title_id = @TitleId,
                                            client_type_id = @ClientTypeId,
                                            name = @Name,
                                            surname = @Surname,
                                            email = @Email,
                                            contact_number = @ContactNumber,
                                            address = @Address
                                        WHERE
                                            client_id = @ClientId";

                        //adding parameters with model data
                        using (SqlCommand updtCmd = new SqlCommand(queryUpdateClient, sqlcon))
                        {
                            updtCmd.Parameters.AddWithValue("@ClientId", model.ClientId);
                            updtCmd.Parameters.AddWithValue("@TitleId", model.TitleId);
                            updtCmd.Parameters.AddWithValue("@ClientTypeId", model.ClientTypeId);
                            updtCmd.Parameters.AddWithValue("@Name", model.Name);
                            updtCmd.Parameters.AddWithValue("@Surname", model.Surname);
                            updtCmd.Parameters.AddWithValue("@Email", model.Email);
                            updtCmd.Parameters.AddWithValue("@ContactNumber", model.ContactNumber);
                            updtCmd.Parameters.AddWithValue("@Address", model.Address);
                            updtCmd.ExecuteNonQuery();
                        }
                    }

                    //query to update username in login_details table
                    string queryUpdateUsername = @"UPDATE login_details                                          
                                                 SET
                                                    username = @Username   
                                                WHERE
                                                    client_id = @ClientId";

                    //username parameter and execute
                    using (SqlCommand updtCmd = new SqlCommand(queryUpdateUsername, sqlcon))
                    {
                        updtCmd.Parameters.AddWithValue("@Username", model.Username);
                        updtCmd.Parameters.AddWithValue("@ClientId", model.ClientId);

                        updtCmd.ExecuteNonQuery();
                    }
                        
                    sqlcon.Close();

                    return RedirectToAction("ViewClient", "Client");
                }
            }
            catch (Exception ex)
            {
                // Pass nothing to the view
                return RedirectToAction("Error", "Client", new { e = ex.Message });
            }
        }
    }
}           
