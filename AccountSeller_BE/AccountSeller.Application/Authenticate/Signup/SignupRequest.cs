using AccountSeller.Application.Common.CQRS;

namespace AccountSeller.Application.Authenticate.Signup
{
    public class SignupRequest : BaseRequest<SignupResponse>
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}