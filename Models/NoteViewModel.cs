namespace LoginPage.Models
{
    public class NoteViewModel
    {
        public int ClientId { get; set; }
        public int NoteId { get; set; }
        public List<NotesModel> notesModels { get; set; }

        public string EmployeeNote { get; set; }
    }
}
