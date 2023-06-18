using CSharper.Services;
using System;

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

            User = new User 
            { 
                Id = Guid.NewGuid(),
                Login = "Not_Logn",
                Password = "User",
                Level = "0"
            };

        }
    }
}
