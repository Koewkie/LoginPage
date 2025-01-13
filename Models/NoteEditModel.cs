namespace LoginPage.Models
{
    public class NoteEditModel
    {
        public int ClientId { get; set; }
        public int NoteId { get; set; }
        public string EmployeeNote { get; set; }
        public string ClientComment { get; set; }
        public DateTime Date { get; set; }
    }
}
