using CSharper.Services;

namespace CSharper.Models
{
    public class AppConfig
    {
        public string ConfigurationsFolder { get; set; }

        public string AppPropertiesFileName { get; set; }

        public static User? User { get; set; }
        public static Subject? Subject { get; set; }

        static AppConfig()
        {
            var userService = new UserService();
           //TODO User = userServiceGetTestUser();
        }
    }
}
