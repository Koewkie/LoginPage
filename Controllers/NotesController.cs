using LoginPage.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Reflection;

namespace LoginPage.Controllers
{
    public class NotesController : Controller
    {
        string conn = "Data Source=localhost;Initial Catalog=CRM_Test_EstianHuman;Integrated Security=True;";

        [HttpGet]
        public IActionResult Notes(int id)
        {
            NoteViewModel vm = new NoteViewModel();
            List<NotesModel> notes = new List<NotesModel>();
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                { 
                    sqlcon.Open();

                    string query = @"SELECT note_id, client_id, employee_note, client_reply, comment_date 
                                    FROM notes WHERE client_id = @ClientId";

                    SqlCommand cmd = new SqlCommand(query, sqlcon);
                    cmd.Parameters.AddWithValue("@ClientId", id);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        var note = new NotesModel();
                        note.NoteId = dr.GetInt32(0);
                        note.EmployeeNote = dr.GetString(2);
                        note.ClientComment = dr.GetString(3);
                        note.Date = dr.GetDateTime(4);

                        notes.Add(note);
                    }
                    sqlcon.Close();
                } 
                vm.ClientId = id;
                vm.notesModels = notes;
                return View(vm);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Client", new { e = ex.Message });
            }           
        }

        [HttpPost]
        public IActionResult AddNote(NoteViewModel vm)
        {
            ModelState.Remove("notesModels");
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please ensure the Note field is filled in";

                return RedirectToAction("Notes", new { id = vm.ClientId});
            }

            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    string query = @"INSERT INTO notes
                                    (client_id, employee_note, client_reply)
                                    VALUES
                                    (@ClientId, @EmployeeNote, @ClientReply)";

                    SqlCommand cmd = new SqlCommand(query,sqlcon);
                    cmd.Parameters.AddWithValue("@ClientId", vm.ClientId);
                    cmd.Parameters.AddWithValue("@EmployeeNote", vm.EmployeeNote);
                    cmd.Parameters.AddWithValue("@ClientReply", "");

                    cmd.ExecuteNonQuery();
                    sqlcon.Close();
                }

                return RedirectToAction("Notes", new { id = vm.ClientId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Client", new { e = ex.Message });
            }

        }

        [HttpGet]
        public IActionResult EditNotesView(int NoteId)
        {
            try
            {
                var note = new NoteEditModel();
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
 
                    string query = @"SELECT note_id, client_id, employee_note, client_reply, comment_date 
                                    FROM notes WHERE note_id = @NoteId";

                    SqlCommand cmd = new SqlCommand(query, sqlcon);
                    cmd.Parameters.AddWithValue("@NoteId", NoteId);
                    SqlDataReader dr = cmd.ExecuteReader();

                   
                    if (dr.Read())
                    {
                        note.NoteId = dr.GetInt32(0);
                        note.ClientId = dr.GetInt32(1);
                        note.EmployeeNote = dr.GetString(2);
                        note.ClientComment = dr.GetString(3);
                        note.Date = dr.GetDateTime(4);
                    }
                    sqlcon.Close();
                }
                return View(note);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Client", new { e = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult EditNote(NoteViewModel vm)
        {
            ModelState.Remove("notesModels");
            if (!ModelState.IsValid)
            {
                string allErrors = string.Join("; ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));

                TempData["ErrorMessage"] = allErrors;
                //TempData["ErrorMessage"] = "Please ensure the Note field is filled in";
                return RedirectToAction("EditNotesView",vm.NoteId);
            }
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    string query = @"UPDATE notes SET 
                                    employee_note = @EmployeeNote,
                                    comment_date = @Date
                                    WHERE 
                                    note_id = @NoteId";

                    SqlCommand cmd = new SqlCommand(query, sqlcon);
                    cmd.Parameters.AddWithValue("@NoteId", vm.NoteId);
                    cmd.Parameters.AddWithValue("@EmployeeNote", vm.EmployeeNote);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now);

                    cmd.ExecuteNonQuery();
                    sqlcon.Close();
                }

                return RedirectToAction("Notes", new { id = vm.ClientId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Client", new { e = ex.Message });
            }
        }
    }
}
