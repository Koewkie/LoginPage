using LoginPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using System.Security.Cryptography;

namespace LoginPage.Controllers
{
    public class AddController : Controller
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
                        return RedirectToAction("Error", "Client", new { e = "ClientTypes is null" });
                    }
                }
            }
            catch (Exception ex)
            {
                // Pass nothing to the view
                return RedirectToAction("Error", "Client", new { e = ex.Message });
            }

            return View(new ClientAddModel());
        }

        [HttpPost]
        public IActionResult AddClient(ClientAddModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please ensure all fields are correctly filled in";
               
                return RedirectToAction("Add");
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
                    string userNameQuery = @"SELECT Count(*) FROM login_details WHERE username = @UserName";

                    using (SqlCommand chkUserName = new SqlCommand(userNameQuery, sqlcon))
                    {
                        chkUserName.Parameters.AddWithValue("@UserName", model.Username);
                        if ((int)chkUserName.ExecuteScalar() > 0)
                        {
                            TempData["ErrorMessage"] = "The username already exists.";
                            
                            return RedirectToAction("Add");
                        }
                    }

                        //query string for adding client
                        string query = @"INSERT INTO basic_client_information 
                                            (profile_picture, title_id, client_type_id, name, surname, 
                                            email, contact_number, address)
                                        VALUES
                                            (@ProfilePicture, @TitleId, @ClientType, @Name, 
                                            @Surname, @Email, @ContactNumber, @Address)
                                        SELECT SCOPE_IDENTITY()";
                    //query string for adding client login details
                    string loginQuery = @"INSERT INTO login_details 
                                        (client_id, username, password_hash, password_salt)
                                    VALUES
                                        (@ClientId, @Username, @PasswordHash, @PasswordSalt)";

                    //adding parameters with model data
                    using (SqlCommand addCmd = new SqlCommand(query, sqlcon))
                    {
                        addCmd.Parameters.AddWithValue("@ProfilePicture", profilePictureData);
                        addCmd.Parameters.AddWithValue("@TitleId", model.TitleId);
                        addCmd.Parameters.AddWithValue("@ClientType", model.ClientTypeId);
                        addCmd.Parameters.AddWithValue("@Name", model.Name);
                        addCmd.Parameters.AddWithValue("@Surname", model.Surname);
                        addCmd.Parameters.AddWithValue("@Email", model.Email);
                        addCmd.Parameters.AddWithValue("@ContactNumber", model.ContactNumber);
                        addCmd.Parameters.AddWithValue("@Address", model.Address);

                        //get new client id
                        var newClientID = addCmd.ExecuteScalar();
                        
                        //add client in login table
                        using (SqlCommand loginCmd = new SqlCommand(loginQuery,sqlcon))
                        {
                            //generate hashed and salted passwords from username for default 
                            var passwordSalt = generateSalt(model.Username);
                            var passwordHash = hashPass(model.Username,passwordSalt);

                            loginCmd.Parameters.AddWithValue("@ClientId", newClientID);
                            loginCmd.Parameters.AddWithValue("@Username", model.Username);
                            loginCmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                            loginCmd.Parameters.AddWithValue("@PasswordSalt", passwordSalt);

                            loginCmd.ExecuteNonQuery();
                        }
                    }
                    sqlcon.Close();

                    return RedirectToAction("ViewClient", "Client");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Client", new { e = ex.Message});
            }
        }

        //methods for generating salt and hash password
        private string generateSalt(string username)
        {
            var usernameBytes = Encoding.UTF8.GetBytes(username);
            return Convert.ToBase64String(usernameBytes);
        }
        private string hashPass(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPass = "{password}{salt}";
                var saltedPassBytes = Encoding.UTF8.GetBytes(saltedPass);
                var hashBytes = sha256.ComputeHash(saltedPassBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
