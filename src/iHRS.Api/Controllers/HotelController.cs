using iHRS.Api.Configuration;
using iHRS.Application.Commands.Hotels;
using iHRS.Application.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace iHRS.Api.Controllers
{
    [JwtAuth]
    [ApiController]
    [Route("hotels")]
    public class HotelController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public HotelController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateHotelCommand command)
        {
            var hotelId = await _commandDispatcher.SendAsync((command));
            return Created($"hotels/{hotelId}", new { hotelId });
        }

        [HttpDelete("{hotelId}")]
        public async Task<IActionResult> Delete(Guid hotelId)
        {
            await _commandDispatcher.SendAsync(new DeleteHotelCommand(hotelId));
            return Ok();
        }
    }
}
