using iHRS.Application.Commands.Auth;
using iHRS.Application.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace iHRS.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public AuthController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SigInModel md)
        {
            var cmd = new SignInCommand(md.EmailAddress, md.Password);
            var result = await _commandDispatcher.SendAsync(cmd);
            return Ok(result);

        }

        public class SigInModel
        {
            public string EmailAddress { get; set; }
            public string Password { get; set; }
        }
    }
}