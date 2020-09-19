using iHRS.Api.Configuration;
using iHRS.Application.Commands.Rooms;
using iHRS.Application.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace iHRS.Api.Controllers
{
    [JwtAuth]
    [ApiController]
    [Route("hotels/{hotelId}/rooms")]
    public class RoomController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public RoomController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Guid hotelId, CreateRoomModel md)
        {
            var roomId = await _commandDispatcher.SendAsync((new CreateRoomCommand(hotelId, md.RoomNumber)));
            return Created($"hotels/{hotelId}/rooms/{roomId}", new { hotelId, roomId });

        }
    }

    public class CreateRoomModel
    {
        public string RoomNumber { get; set; }
    }
}
