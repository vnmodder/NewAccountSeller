using AccountSeller.Application.Common.CQRS;
using System.ComponentModel.DataAnnotations;

namespace AccountSeller.Application.UserManagement.Commands.UpdateUser
{
    public class UpdateUserCommand : BaseRequest<UpdateUserResponse>
    {
        [Required(ErrorMessage = "MoveTime is required")]
        public DateTime MoveTime { get; set; }
        public bool UserOverrideConfirmed { get; set; } = false;

        [Required(ErrorMessage = "UserId is required")]
        public string? Name { get; set; }
        public string UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}
