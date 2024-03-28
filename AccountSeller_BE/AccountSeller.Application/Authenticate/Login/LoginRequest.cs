using AccountSeller.Application.Common.CQRS;
using System.ComponentModel.DataAnnotations;

namespace AccountSeller.Application.Authenticate.Login
{
    public class LoginRequest : BaseRequest<LoginResponse>
    {
        public string? LoginId { get; set; }

        public string? Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
