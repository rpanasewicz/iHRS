using iHRS.Application.Commands.Customers.Auth;
using iHRS.Application.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace iHRS.Api.Controllers
{
    [ApiController]
    [Route("/customers")]
    public class CustomerController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public CustomerController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost("/token")]
        public async Task<IActionResult> ValidateCustomer(ValidateCustomerCommand cmd)
        {
            await _commandDispatcher.SendAsync(cmd);
            return Ok();
        }

        [HttpGet("/token")]
        public async Task<IActionResult> SendLoginEmail([FromQuery] string linkRef)
        {
            return Ok(await _commandDispatcher.SendAsync(new GetCustomerTokenCommand(linkRef)));
        }
    }
}
