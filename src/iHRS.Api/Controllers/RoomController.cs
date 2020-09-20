using iHRS.Api.Configuration;
using iHRS.Application.Commands.Rooms;
using iHRS.Application.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using iHRS.Domain.Models;
using Microsoft.Extensions.Logging;

namespace iHRS.Api.Controllers
{
    [JwtAuth]
    [ApiController]
    [Route("hotels/{hotelId}/rooms")]
    public class RoomController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ILogger<RoomController> _logger;

        public RoomController(ICommandDispatcher commandDispatcher, ILogger<RoomController> logger)
        {
            _commandDispatcher = commandDispatcher;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateRoomCommand cmd)
        {
            var roomId = await _commandDispatcher.SendAsync(cmd);
            return Created($"hotels/{cmd.HotelId}/rooms/{roomId}", new { cmd.HotelId, roomId });

        }

    }
}
