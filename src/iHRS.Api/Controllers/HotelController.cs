using iHRS.Api.Configuration;
using iHRS.Application.Commands.Hotels;
using iHRS.Application.Common;
using iHRS.Application.Queries;
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
        private readonly HotelQueries _hotelQueries;

        public HotelController(ICommandDispatcher commandDispatcher, HotelQueries hotelQueries)
        {
            _commandDispatcher = commandDispatcher;
            _hotelQueries = hotelQueries;
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

        [HttpPost("{hotelId}/messageTemplates")]
        public async Task<IActionResult> CreateMessageTemplate(CreateMessageTemplateCommand cmd)
        {
            var templateId = await _commandDispatcher.SendAsync(cmd);
            return Created($"hotels/{cmd.HotelId}/messageTemplates/{templateId}", new { cmd.HotelId, templateId });
        }

        [HttpGet]
        public IActionResult GetAll()
            => Ok(_hotelQueries.GetAll());
    }
}
