//using iHRS.Api.Configuration;
//using iHRS.Application.Auth;
//using iHRS.Application.Commands.Customers.Auth;
//using iHRS.Application.Common;
//using iHRS.Application.Queries;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Threading.Tasks;

//namespace iHRS.Api.Controllers
//{
//    [ApiController]
//    [Route("customers")]
//    public class CustomerController : Controller
//    {
//        private readonly ICommandDispatcher _commandDispatcher;
//        private readonly IAuthProvider _auth;
//        private readonly ICustomerReservationQueries _queries;

//        private Guid CustomerId =>
//            _auth.CustomerId != Guid.Empty ? _auth.CustomerId : throw new UnauthorizedAccessException();

//        public CustomerController(ICommandDispatcher commandDispatcher, IAuthProvider auth, ICustomerReservationQueries queries)
//        {
//            _commandDispatcher = commandDispatcher;
//            _auth = auth;
//            _queries = queries;
//        }

//        [HttpPost("token")]
//        public async Task<IActionResult> ValidateCustomer(ValidateCustomerCommand cmd)
//        {
//            await _commandDispatcher.SendAsync(cmd);
//            return Ok();
//        }

//        [HttpGet("token")]
//        public async Task<IActionResult> SendLoginEmail([FromQuery] string linkRef)
//        {
//            return Ok(await _commandDispatcher.SendAsync(new GetCustomerTokenCommand(linkRef)));
//        }

//        [JwtAuth]
//        [HttpGet("reservations")]
//        public IActionResult GetAllReservations() => Ok(_queries.GetAll(CustomerId));
//    }
//}
