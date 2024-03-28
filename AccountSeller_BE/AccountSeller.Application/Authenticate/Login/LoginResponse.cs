using Newtonsoft.Json;

namespace AccountSeller.Application.Authenticate.Login
{
    public class LoginResponse
    {
        //public Guid? Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
        [JsonIgnore]
        public bool LoginSuccessFlag { get; set; }
    }
}
