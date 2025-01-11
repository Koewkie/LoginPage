using LoginPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;

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
                        return RedirectToAction("Error", new { e = "ClientTypes is null" });
                    }
                }
            }
            catch (Exception ex)
            {
                // Pass nothing to the view
                return RedirectToAction("Error", new { e = ex.Message });
            }

            return View(new ClientAddModel());
        }
    }

}
