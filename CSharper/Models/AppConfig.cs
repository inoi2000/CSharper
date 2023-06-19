using CSharper.Services;
using System;

namespace CSharper.Models
{
    public class AppConfig
    {
        public string ConfigurationsFolder { get; set; }

        public string AppPropertiesFileName { get; set; }

        public static User? User { get; set; }
        private static User _defaultUser;
        public static Subject? Subject { get; set; }

        static AppConfig()
        {
            _defaultUser = CreateDefaultUser();
            User = _defaultUser;
        }

        private static User CreateDefaultUser()
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Login = "Неавторизованный пользователь",
                Password = "User",
                Level = "0"
            };
        }

        public static bool IsСurrentUserDefault()
        {
            return User.Equals(_defaultUser);
        }


    }
}
