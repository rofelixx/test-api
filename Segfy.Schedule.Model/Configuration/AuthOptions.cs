namespace Segfy.Schedule.Model.Configuration
{
    public class AuthOptions
    {
        public string Authority { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string CookieName { get; set; }
    }
}