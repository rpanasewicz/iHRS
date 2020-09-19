using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iHRS.Application.Commands.Auth;
using iHRS.Application.Common;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(SigUpModel md)
        {
            var cmd = new SignUpCommand(md.FirstName, md.LastName, md.Email, md.Password, md.DateOfBirth);
            await _commandDispatcher.SendAsync(cmd);
            return Ok();

        }

        public class SigInModel
        {
            public string EmailAddress { get; set; }
            public string Password { get; set; }
        }


        public class SigUpModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public DateTime DateOfBirth { get; set; }
        }
    }
}