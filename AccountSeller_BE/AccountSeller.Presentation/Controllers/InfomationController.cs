using AccountSeller.Application.Information.Commands.AddInfomation;
using AccountSeller.Application.Information.Commands.DeleteInfomation;
using AccountSeller.Application.Information.Queries.GetInformationList;
using AccountSeller.Application.UserManagement.Queries.GetUserInformationList;
using AccountSeller.Domain.Exceptions;
using AccountSeller.Domain.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountSeller.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InfomationController : AccountSellerController
    {
        /// <summary>
        /// Adds the information.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-information")]
        public async Task<IActionResult> AddInformation([FromBody] AddInfomationCommand request)
        {
            var response = await Mediator.Send(request);
            return Response(response);
        }

        /// <summary>
        /// Gets the information list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("information-list")]
        public async Task<IActionResult> GetInformationList([FromQuery] GetInformationListRequest request)
        {
            var response = await Mediator.Send(request);
            return Response(response);
        }

        [HttpDelete]
        [Route("delete-information")]
        public async Task<IActionResult> DeleteInformation([FromBody] DeleteInfomationCommand request)
        {
            var response = await Mediator.Send(request);
            return Response(response);
        }
    }
}
