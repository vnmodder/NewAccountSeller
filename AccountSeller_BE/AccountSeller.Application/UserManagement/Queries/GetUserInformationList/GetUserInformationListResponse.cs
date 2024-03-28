using AccountSeller.Application.UserManagement.Queries.GetUserInformationList.Models;

namespace AccountSeller.Application.UserManagement.Queries.GetUserInformationList
{
    public class GetUserInformationListResponse
    {
        public bool IsSuccess { get; set; } = true;
        public string ErrorCode { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;

        public List<UserInformationModel> Users { get; set; } = new List<UserInformationModel>();
    }
}
