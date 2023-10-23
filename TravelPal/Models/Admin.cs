namespace TravelPal.Models
{
    public class Admin : IUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Country Location { get; set; }
    }
}
