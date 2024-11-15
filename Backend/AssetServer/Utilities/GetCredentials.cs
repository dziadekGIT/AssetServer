using Newtonsoft.Json;

namespace PasswordResetAPI.Utilities
{

    public class User
    {
        public string login { get; set; }
        public string password { get; set; }

    }

    public class GetCredentials
    {
        private string pathToJson = "config.json";

        public User getUser()
        {
            var credits = File.ReadAllText(pathToJson);
            User adminCredentials = JsonConvert.DeserializeObject<User>(credits);
            return adminCredentials;
        }
    }

}