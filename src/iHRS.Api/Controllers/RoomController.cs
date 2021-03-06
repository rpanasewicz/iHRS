﻿using iHRS.Api.Configuration;
using iHRS.Application.Commands.Rooms;
using iHRS.Application.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace iHRS.Api.Controllers
{
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

        [JwtAuth]
        [HttpPost]
        public async Task<IActionResult> Post(CreateRoomCommand cmd)
        {
            var roomId = await _commandDispatcher.SendAsync(cmd);
            return Created($"hotels/{cmd.HotelId}/rooms/{roomId}", new { cmd.HotelId, roomId });

        }

        [HttpPost("{roomId}")]
        public async Task<IActionResult> CreateReservation(MakeReservationCommand cmd)
        {
            var reservationId = await _commandDispatcher.SendAsync(cmd);
            return Created($"hotels/{cmd.HotelId}/rooms/{cmd.RoomId}/reservations/{reservationId}", new { cmd.HotelId, cmd.RoomId, reservationId });
        }
    }
}
