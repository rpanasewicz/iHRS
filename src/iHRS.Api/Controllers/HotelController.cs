using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iHRS.Application.Commands;
using iHRS.Application.Commands.Hotels;
using iHRS.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace iHRS.Api.Controllers
{
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
            await _commandDispatcher.SendAsync((command));
            return Ok();
        }

        [HttpDelete("{hotelId}")]
        public async Task<IActionResult> Delete(Guid hotelId)
        {
            await _commandDispatcher.SendAsync(new DeleteHotelCommand(hotelId));
            return Ok();
        }
    }
}
