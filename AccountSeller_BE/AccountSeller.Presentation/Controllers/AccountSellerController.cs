using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountSeller.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountSellerController : ControllerBase
    {
        private const int HTTP_SUCCESS = 200;
        private ISender _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();

        protected ObjectResult Response<T>(T value, int statusCode)
        {
            return StatusCode(statusCode, new ResponseModel(value));
        }

        protected ObjectResult Response<T>(T value)
        {
            return StatusCode(HTTP_SUCCESS, new ResponseModel(value));
        }

        protected class ResponseModel
        {
            public ResponseModel(object result)
            {
                Result = result;
            }

            public object Result { get; set; }
        }
    }
}
