namespace LoginPage.Models
{
    public class ClientAddModel
    {
        public TitleModel Title { get; set; }
        public ClientTypeModel ClientType { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public IFormFile ProfilePicture { get; set; }
    }
}
