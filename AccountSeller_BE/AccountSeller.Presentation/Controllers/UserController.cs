using AccountSeller.Application.UserManagement.Queries.GetUserInformationList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountSeller.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : AccountSellerController
    {
        /// <summary>
        /// Get list all users
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("user-information-list")]
        public async Task<IActionResult> GetUserInformationList([FromQuery] GetUserInformationListRequest request)
        {
            var response = await Mediator.Send(request);
            return Response(response);
        }
    }
}
