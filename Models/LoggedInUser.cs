namespace Models
{
    public class LoggedInUser
    {
        public User User { get; set; }

        public string AccessToken { get; set; }

        public AppSettings AppSettings { get; set; }
    }
}
