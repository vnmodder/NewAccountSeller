using AccountSeller.Application.Common.CQRS;
using System.ComponentModel.DataAnnotations;

namespace AccountSeller.Application.UserManagement.Commands.DeleteUser
{
    public class DeleteUserCommand : BaseRequest<DeleteUserResponse>
    {
        [Required(ErrorMessage = "UserId is required")]
        public string UserName { get; set; }
    }
}
