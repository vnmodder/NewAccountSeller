namespace AccountSeller.Application.UserManagement.Queries.GetUserInformationList.Models
{
    public class UserInformationModel
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string Mail { get; set; }
        public DateTime? UpdateDate { get; set; } 
    }
}
