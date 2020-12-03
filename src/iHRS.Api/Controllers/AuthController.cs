using iHRS.Application.Commands.Auth;
using iHRS.Application.Common;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> SignIn(SignInCommand cmd)
        {
            var result = await _commandDispatcher.SendAsync(cmd);
            return Ok(result);
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand cmd)
        {
            await _commandDispatcher.SendAsync(cmd);
            return Ok();
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand cmd)
        {
            await _commandDispatcher.SendAsync(cmd);
            return Ok();
        }

        [HttpPost("setupPassword")]
        public async Task<IActionResult> SetupPassword(SetupPasswordCommand cmd)
        {
            await _commandDispatcher.SendAsync(cmd);
            return Ok();
        }
    }
}