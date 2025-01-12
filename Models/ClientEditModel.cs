namespace LoginPage.Models
{
    public class ClientEditModel
    {
        //public TitleModel Title { get; set; }
        //public ClientTypeModel ClientType { get; set; }
        public int ClientId { get; set; }
        public int TitleId { get; set; }
        public int ClientTypeId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public IFormFile ProfilePicture { get; set; }
    }
}
