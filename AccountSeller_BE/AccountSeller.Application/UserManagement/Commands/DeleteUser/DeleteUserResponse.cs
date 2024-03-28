namespace AccountSeller.Application.UserManagement.Commands.DeleteUser
{
    public class DeleteUserResponse
    {
        public bool IsSuccess { get => String.IsNullOrEmpty(Message); }
        public string? ErrorCode { get; set; }
        public string? Message { get; set; }
    }
}
